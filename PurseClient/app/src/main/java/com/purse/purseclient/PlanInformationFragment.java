package com.purse.purseclient;


import android.app.ProgressDialog;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTransaction;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.purse.array_adapter.PlanFlightAdapter;
import com.purse.entity.Flight;
import com.purse.entity.Plan;
import com.purse.helper.Constants;
import com.purse.helper.WorkflowStatus;
import com.purse.services.RestService;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.http.Body;

public class PlanInformationFragment extends Fragment implements android.view.View.OnClickListener {
    private int planCode;
    private View view;
    private ProgressDialog progress;
    private TextView fieldCode;
    private TextView fieldName;
    private TextView fieldPlannedBudget;
    private TextView fieldStartDate;
    private TextView fieldEndDate;
    private TextView fieldActualBudget;

    private ListView flightsPlanListView;

    private CheckBox useHowTemplate;
    private CheckBox useLastDataPlan;

    private Button actualizePlan;
    private Button deletePlan;
    private Button editPlan;
    private Button addFlight;
    private Button undeletePlan;
    private Button useTemplateHowPlan;

    private boolean isDeletedFlight;
    private boolean isTimeActualize;

    public PlanInformationFragment() {

    }

    public void setPlanCode(int planCode) {
        this.planCode = planCode;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_plan_information, container, false);
        fieldCode = (TextView) view.findViewById(R.id.code_plan_info);
        fieldName = (TextView) view.findViewById(R.id.name_plan_info);
        fieldPlannedBudget = (TextView) view.findViewById(R.id.planned_budget_plan_info);
        fieldStartDate = (TextView) view.findViewById(R.id.start_date_plan_info);
        fieldEndDate = (TextView) view.findViewById(R.id.end_date_plan_info);
        fieldActualBudget = (TextView) view.findViewById(R.id.actual_budget_plan_info);
        progress = new ProgressDialog(view.getContext());

        useHowTemplate = (CheckBox) view.findViewById(R.id.use_how_template);
        useHowTemplate.setOnClickListener(this);
        useLastDataPlan = (CheckBox) view.findViewById(R.id.use_planned_budget_last_plan);
        useLastDataPlan.setOnClickListener(this);

        addFlight = (Button) view.findViewById(R.id.add_flight_plan_btn);
        addFlight.setOnClickListener(this);

        deletePlan = (Button) view.findViewById(R.id.delete_plan_btn);
        deletePlan.setOnClickListener(this);

        actualizePlan = (Button) view.findViewById(R.id.actualize_plan_btn);
        actualizePlan.setOnClickListener(this);

        editPlan = (Button) view.findViewById(R.id.editor_plan_btn);
        editPlan.setOnClickListener(this);

        undeletePlan = (Button) view.findViewById(R.id.undelete_plan_btn);
        undeletePlan.setOnClickListener(this);

        useTemplateHowPlan = (Button) view.findViewById(R.id.use_template_how_plan);
        useTemplateHowPlan.setOnClickListener(this);

        flightsPlanListView = (ListView) view.findViewById(R.id.flights_plan_list_view);

        initializationPlanData();

        return view;
    }

    private void initializationPlanData() {

        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();

        actualizePlan.setVisibility(View.GONE);
        editPlan.setVisibility(View.GONE);
        deletePlan.setVisibility(View.GONE);
        addFlight.setVisibility(View.GONE);

        Call<Plan> call_plan = RestService.getService().plan(planCode);

        call_plan.enqueue(new Callback<Plan>() {
            @Override
            public void onResponse(Call<Plan> call, Response<Plan> response) {
                progress.dismiss();
                if (response.isSuccessful()) {
                    Plan plan = response.body();

                    if (plan != null) {
                        fieldCode.setText(String.valueOf(plan.Code));
                        fieldName.setText(plan.Name);

                        Date startDate = new Date(plan.StartDate);
                        DateFormat dateFormat = new SimpleDateFormat("dd/MM/yyyy");
                        fieldStartDate.setText(dateFormat.format(startDate));

                        Date endDate = new Date(plan.EndDate);
                        fieldEndDate.setText(dateFormat.format(endDate));

                        boolean isOwner = Constants.userCode == plan.OwnerCode;
                        boolean isExecutor = Constants.userCode == plan.ExecutorCode;
                        boolean isApprove = plan.Status == WorkflowStatus.Approved;
                        boolean isDeleted = plan.Status == WorkflowStatus.Deleted;

                        Date now = getStartOfDay();
                        isTimeActualize = startDate.before(now) && plan.ExecutorCode == Constants.userCode;
                        if (!isApprove && !isDeleted) {
                            actualizePlan.setVisibility(isTimeActualize ? View.VISIBLE : View.GONE);
                            editPlan.setVisibility(isOwner ? View.VISIBLE : View.GONE);
                            deletePlan.setVisibility(isOwner ? View.VISIBLE : View.GONE);
                            addFlight.setVisibility(View.VISIBLE);
                        }

                        if (isDeleted)
                            undeletePlan.setVisibility(View.VISIBLE);


                        isDeletedFlight = isExecutor || isOwner;


                        fieldActualBudget.setText(String.valueOf(plan.ActualBudget));
                        fieldActualBudget.setVisibility(startDate.before(now) ? View.VISIBLE : View.GONE);
                        if (!isApprove)
                            fieldPlannedBudget.setText(String.valueOf(plan.PlannedBudget));
                        else
                            fieldPlannedBudget.setVisibility(View.GONE);

                        if (isApprove && isOwner)
                            initTemplateData();

                    } else {
                        Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
                    }
                } else {
                    Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void onFailure(Call<Plan> call, Throwable t) {

            }
        });

        try {
            Thread.sleep(500);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        Call<List<Flight>> call_flights = RestService.getService().flightsPlan(planCode);

        call_flights.enqueue(new Callback<List<Flight>>() {
            @Override
            public void onResponse(Call<List<Flight>> call, Response<List<Flight>> response) {
                if (response.isSuccessful()) {
                    List<Flight> flights = response.body();
                    if (flights == null) {
                        Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
                        return;
                    }

                    boolean isDelete = false;
                    for (Flight flight : flights) {
                        if (flight.Status == WorkflowStatus.InPlanned) {
                            actualizePlan.setVisibility(View.GONE);
                            break;
                        } else if (flight.Status == WorkflowStatus.Deleted) {
                            isDelete = true;
                            break;
                        }
                    }

                    PlanFlightAdapter planFlightAdapter = new PlanFlightAdapter(view.getContext(), R.layout.view_flights_entity, flights);

                    flightsPlanListView.setAdapter(planFlightAdapter);

                    if (!isDelete) {
                        flightsPlanListView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
                            @Override
                            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                                int flightCode = Integer.valueOf(((TextView) view.findViewById(R.id.flight_entity_code)).getText().toString());

                                FlightInformationFragment fragment = new FlightInformationFragment();
                                fragment.setInformation(flightCode, fieldName.getText().toString(), isTimeActualize, isDeletedFlight);
                                FragmentTransaction fragmentTransaction = getActivity().getSupportFragmentManager().beginTransaction();
                                fragmentTransaction.replace(R.id.content_frame, fragment);
                                fragmentTransaction.addToBackStack(null);
                                fragmentTransaction.commit();
                            }
                        });
                    }
                } else {
                    Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void onFailure(Call<List<Flight>> call, Throwable t) {

            }
        });

    }

    private void initTemplateData() {
        Call<List<Boolean>> call = RestService.getService().getInfoTemplate(planCode);
        call.enqueue(new Callback<List<Boolean>>() {
            @Override
            public void onResponse(Call<List<Boolean>> call, Response<List<Boolean>> response) {
                if (response.isSuccessful()) {
                    List<Boolean> info = response.body();
                    if (info == null)
                        return;
                    useHowTemplate.setVisibility(View.VISIBLE);
                    useHowTemplate.setChecked(info.get(0));
                    if (info.get(0)) {
                        useTemplateHowPlan.setVisibility(View.VISIBLE);
                        useLastDataPlan.setVisibility(View.VISIBLE);
                        useLastDataPlan.setChecked(info.get(1));
                    }
                }
            }

            @Override
            public void onFailure(Call<List<Boolean>> call, Throwable t) {

            }
        });
    }

    @Override
    public void onClick(View v) {
        if (v == view.findViewById(R.id.add_flight_plan_btn)) {

            String name = fieldName.getText().toString();

            FlightEditorFragment fragment = new FlightEditorFragment();
            fragment.setInfoPlan(planCode, name);
            FragmentTransaction fragmentTransaction = getActivity().getSupportFragmentManager().beginTransaction();
            fragmentTransaction.replace(R.id.content_frame, fragment);
            fragmentTransaction.addToBackStack(null);
            fragmentTransaction.commit();

        } else if (v == view.findViewById(R.id.actualize_plan_btn)) {
            approvePlan();
        } else if (v == view.findViewById(R.id.editor_plan_btn)) {
            editorPlan(planCode, false);
        } else if (v == view.findViewById(R.id.delete_plan_btn)) {
            deletePlan();
        } else if (v == view.findViewById(R.id.undelete_plan_btn)) {
            undeletePlan();
        } else if (v == view.findViewById(R.id.use_how_template) || v == view.findViewById(R.id.use_planned_budget_last_plan)) {
            updateTemplate();
        } else if (v == view.findViewById(R.id.use_template_how_plan)) {
            createPlan();
        }
    }

    private void createPlan() {
        Call<Integer> call = RestService.getService().createPlanUseTemplate(planCode);
        call.enqueue(new Callback<Integer>() {
            @Override
            public void onResponse(Call<Integer> call, Response<Integer> response) {
                Toast.makeText(view.getContext(), response.message(), Toast.LENGTH_LONG).show();
                if (response.isSuccessful()) {
                    Integer code = response.body();
                    if (code == null)
                        return;
                    editorPlan(code, true);
                }
            }

            @Override
            public void onFailure(Call<Integer> call, Throwable t) {

            }
        });
    }

    private void updateTemplate() {
        Call<Boolean> call = RestService.getService().getUpdateTemplate(planCode, useHowTemplate.isChecked(), useLastDataPlan.isChecked());
        call.enqueue(new Callback<Boolean>() {
            @Override
            public void onResponse(Call<Boolean> call, Response<Boolean> response) {
                if (response.isSuccessful()) {
                    Boolean isOk = response.body();
                    if (isOk != null && isOk) {
                        Toast.makeText(view.getContext(), "OK", Toast.LENGTH_LONG).show();
                        useLastDataPlan.setVisibility(useHowTemplate.isChecked() ? View.VISIBLE : View.GONE);
                        useTemplateHowPlan.setVisibility(useHowTemplate.isChecked() ? View.VISIBLE : View.GONE);
                        useLastDataPlan.setChecked(useHowTemplate.isChecked() && useLastDataPlan.isChecked());
                    }
                }
            }

            @Override
            public void onFailure(Call<Boolean> call, Throwable t) {

            }
        });
    }

    //useHowTemplate.isChecked()
    private void undeletePlan() {
        Call<Boolean> call = RestService.getService().undeletePlan(planCode);

        call.enqueue(new Callback<Boolean>() {
            @Override
            public void onResponse(Call<Boolean> call, Response<Boolean> response) {
                if (response.isSuccessful()) {
                    initializationPlanData();
                }
            }

            @Override
            public void onFailure(Call<Boolean> call, Throwable t) {

            }
        });
    }

    private void deletePlan() {
        Call<Boolean> call = RestService.getService().deletePlan(planCode);

        call.enqueue(new Callback<Boolean>() {
            @Override
            public void onResponse(Call<Boolean> call, Response<Boolean> response) {
                if (response.isSuccessful()) {
                    PlanFragment fragment = new PlanFragment();
                    FragmentTransaction fragmentTransaction = getActivity().getSupportFragmentManager().beginTransaction();
                    fragmentTransaction.replace(R.id.content_frame, fragment);
                    fragmentTransaction.addToBackStack(null);
                    fragmentTransaction.commit();
                }
            }

            @Override
            public void onFailure(Call<Boolean> call, Throwable t) {

            }
        });
    }

    private void editorPlan(int code, boolean isTemplate) {
        PlanEditorFragment fragment = new PlanEditorFragment();
        fragment.setPlanCode(code, isTemplate);
        FragmentTransaction fragmentTransaction = getActivity().getSupportFragmentManager().beginTransaction();
        fragmentTransaction.replace(R.id.content_frame, fragment);
        fragmentTransaction.addToBackStack(null);
        fragmentTransaction.commit();
    }

    private void approvePlan() {
        Call<Boolean> call = RestService.getService().approvePlan(planCode);

        call.enqueue(new Callback<Boolean>() {
            @Override
            public void onResponse(Call<Boolean> call, Response<Boolean> response) {
                if (response.isSuccessful()) {
                    initializationPlanData();
                }
            }

            @Override
            public void onFailure(Call<Boolean> call, Throwable t) {

            }
        });
    }

    private Date getStartOfDay() {
        Calendar calendar = Calendar.getInstance();
        int year = calendar.get(Calendar.YEAR);
        int month = calendar.get(Calendar.MONTH);
        int day = calendar.get(Calendar.DATE);
        calendar.set(year, month, day, 0, 0, 0);
        return calendar.getTime();
    }
}
