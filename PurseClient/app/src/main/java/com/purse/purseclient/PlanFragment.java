package com.purse.purseclient;


import android.app.DatePickerDialog;
import android.app.ProgressDialog;
import android.location.GpsSatellite;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.text.format.DateUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.purse.array_adapter.PlanAdapter;
import com.purse.entity.CategoryService;
import com.purse.entity.Plan;
import com.purse.entity.UserData;
import com.purse.helper.Constants;
import com.purse.helper.FilterData;
import com.purse.helper.MultiSpinner;
import com.purse.helper.WorkflowStatus;
import com.purse.services.RestService;

import java.util.Calendar;
import java.util.LinkedList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class PlanFragment extends Fragment implements AdapterView.OnItemClickListener {
    private FragmentManager fragmentManager;
    private ListView planListView;
    private ListView templateListView;
    private ProgressDialog progress;
    private Calendar dateStartInterval;
    private Calendar dateEndInterval;
    private TextView fieldDateStartInterval;
    private TextView fieldDateEndInterval;
    private View view;
    private FilterData filter;

    private MultiSpinner spinnerCategory;
    private MultiSpinner spinnerOwner;
    private MultiSpinner spinnerExecutor;
    private MultiSpinner spinnerStatus;

    public PlanFragment() {
        dateStartInterval = getStartOfDay();
        dateEndInterval = getEndOfDay();
        filter = new FilterData();
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {


        view = inflater.inflate(R.layout.fragment_plan, container, false);

        progress = new ProgressDialog(view.getContext());

        fieldDateStartInterval = (TextView) view.findViewById(R.id.interval_start_date);
        fieldDateStartInterval.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                new DatePickerDialog(view.getContext(), dateStart,
                        dateStartInterval.get(Calendar.YEAR),
                        dateStartInterval.get(Calendar.MONTH),
                        dateStartInterval.get(Calendar.DAY_OF_MONTH))
                        .show();
            }
        });
        fieldDateEndInterval = (TextView) view.findViewById(R.id.interval_end_date);
        fieldDateEndInterval.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                new DatePickerDialog(view.getContext(), dateEnd,
                        dateEndInterval.get(Calendar.YEAR),
                        dateEndInterval.get(Calendar.MONTH),
                        dateEndInterval.get(Calendar.DAY_OF_MONTH))
                        .show();
            }
        });
        setInitialDateTime();

        Button createButton = (Button) view.findViewById(R.id.create_plan);
        createButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                PlanEditorFragment fragment = new PlanEditorFragment();
                FragmentTransaction fragmentTransaction = getActivity().getSupportFragmentManager().beginTransaction();
                fragmentTransaction.replace(R.id.content_frame, fragment);
                fragmentTransaction.addToBackStack(null);
                fragmentTransaction.commit();
            }
        });

        Button sendFilter = (Button) view.findViewById(R.id.send_filter);
        sendFilter.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                initialPlanListView();
            }
        });

        planListView = (ListView) view.findViewById(R.id.plan_list_view);
        planListView.setOnItemClickListener(this);

        templateListView = (ListView) view.findViewById(R.id.template_list_view);
        templateListView.setOnItemClickListener(this);

        setDataSpinner();

        TextView actionFilter = (TextView) view.findViewById(R.id.filter_action);
        actionFilter.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                LinearLayout layout = (LinearLayout) view.findViewById(R.id.all_data_filter);
                int value = layout.getVisibility();
                layout.setVisibility(value == View.VISIBLE ? View.GONE : View.VISIBLE);
            }
        });

        TextView planView = (TextView) view.findViewById(R.id.view_plans);
        planView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                LinearLayout plan = (LinearLayout) view.findViewById(R.id.plan);
                plan.setVisibility(plan.getVisibility() == View.GONE ? View.VISIBLE : View.GONE);
                LinearLayout template = (LinearLayout) view.findViewById(R.id.template);
                template.setVisibility(View.GONE);
                loadPlans(plan.getVisibility() == View.VISIBLE, false);
            }
        });
        TextView templateView = (TextView) view.findViewById(R.id.view_template);
        templateView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                LinearLayout template = (LinearLayout) view.findViewById(R.id.template);
                template.setVisibility(template.getVisibility() == View.GONE ? View.VISIBLE : View.GONE);
                LinearLayout plan = (LinearLayout) view.findViewById(R.id.plan);
                plan.setVisibility(View.GONE);
                loadPlans(false, template.getVisibility() == View.VISIBLE);
            }
        });
        return view;
    }

    private void loadPlans(boolean isPlan, boolean isTemplate) {
        if (isPlan)
            initialPlanListView();
        else if (isTemplate)
            initialTemplateListView();
    }

    private void initialTemplateListView() {
        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();

        Call<List<Plan>> call = RestService.getService().getTemplatesPlan();
        call.enqueue(new Callback<List<Plan>>() {
            @Override
            public void onResponse(Call<List<Plan>> call, Response<List<Plan>> response) {
                progress.dismiss();
                if (response.isSuccessful()) {

                    List<Plan> plans = response.body();

                    PlanAdapter planAdapter = new PlanAdapter(view.getContext(), R.layout.view_plan_entity, plans);

                    templateListView.setAdapter(planAdapter);

                } else {
                    Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void onFailure(Call<List<Plan>> call, Throwable t) {
                progress.dismiss();
            }
        });

    }


    private void setDataSpinner() {

        spinnerCategory = (MultiSpinner) view.findViewById(R.id.filter_category);
        Call<List<CategoryService>> callCategory = RestService.getService().getCategories();
        callCategory.enqueue(new Callback<List<CategoryService>>() {
            @Override
            public void onResponse(Call<List<CategoryService>> call, Response<List<CategoryService>> response) {
                if (response.isSuccessful() && response.body() != null) {
                    List<Integer> csCodes = new LinkedList<>();
                    List<String> csName = new LinkedList<>();
                    for (CategoryService cs : response.body()) {
                        csCodes.add(cs.Code);
                        csName.add(cs.Name);
                    }

                    spinnerCategory.setItems(csName, csCodes);
                }
            }

            @Override
            public void onFailure(Call<List<CategoryService>> call, Throwable t) {

            }
        });

        spinnerExecutor = (MultiSpinner) view.findViewById(R.id.filter_executor);
        spinnerOwner = (MultiSpinner) view.findViewById(R.id.filter_owner);
        if (Constants.familyCode == Constants.DEFAULT_CODE) {
            spinnerExecutor.setVisibility(View.GONE);
            spinnerOwner.setVisibility(View.GONE);
        } else {
            Call<List<UserData>> callUsers = RestService.getService().usersList();

            callUsers.enqueue(new Callback<List<UserData>>() {
                @Override
                public void onResponse(Call<List<UserData>> call, Response<List<UserData>> response) {
                    if (response.isSuccessful() & response.body() != null) {
                        List<Integer> usersCodes = new LinkedList<>();
                        List<String> usersName = new LinkedList<>();
                        int index = 0;
                        List<UserData> users = response.body();
                        for (int i = 0; i < users.size(); i++) {
                            UserData user = users.get(i);
                            if (user.Code == Constants.familyCode)
                                index = i;
                            usersCodes.add(user.Code);
                            usersName.add(String.format("%1$s %2$s (%3$s)", user.FirstName, user.LastName, user.NickName));
                        }
                        spinnerOwner.setItems(usersName, usersCodes, Constants.userName, index);
                        spinnerExecutor.setItems(usersName, usersCodes, Constants.userName, index);
                    }
                }

                @Override
                public void onFailure(Call<List<UserData>> call, Throwable t) {

                }
            });
        }

        spinnerStatus = (MultiSpinner) view.findViewById(R.id.filter_status);

        List<Integer> statusCodes = new LinkedList<>();
        statusCodes.add(WorkflowStatus.InPlanned);
        statusCodes.add(WorkflowStatus.Approved);
        statusCodes.add(WorkflowStatus.Deleted);

        List<String> statusName = new LinkedList<>();
        statusName.add("В процессе");
        statusName.add("Выплоненные");
        statusName.add("Удаленные");
        spinnerStatus.setItems(statusName, statusCodes, "В процессе", 0);
    }

    private Calendar getStartOfDay() {
        Calendar calendar = Calendar.getInstance();
        int year = calendar.get(Calendar.YEAR);
        int month = calendar.get(Calendar.MONTH);
        int day = calendar.get(Calendar.DATE) - 30;
        calendar.set(year, month, day, 0, 0, 0);
        return calendar;
    }

    private Calendar getEndOfDay() {
        Calendar calendar = Calendar.getInstance();
        int year = calendar.get(Calendar.YEAR);
        int month = calendar.get(Calendar.MONTH);
        int day = calendar.get(Calendar.DATE);
        calendar.set(year, month, day, 23, 59, 59);
        return calendar;
    }

    private void initialPlanListView() {
        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();

        filter = getFilter();

        Call<List<Plan>> call = RestService.getService().listPlan(filter);
        call.enqueue(new Callback<List<Plan>>() {
            @Override
            public void onResponse(Call<List<Plan>> call, Response<List<Plan>> response) {
                progress.dismiss();
                if (response.isSuccessful()) {

                    List<Plan> plans = response.body();

                    PlanAdapter planAdapter = new PlanAdapter(view.getContext(), R.layout.view_plan_entity, plans);

                    planListView.setAdapter(planAdapter);

                } else {
                    Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void onFailure(Call<List<Plan>> call, Throwable t) {
                progress.dismiss();
            }
        });

    }

    private DatePickerDialog.OnDateSetListener dateEnd = new DatePickerDialog.OnDateSetListener() {
        public void onDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth) {
            dateEndInterval.set(Calendar.YEAR, year);
            dateEndInterval.set(Calendar.MONTH, monthOfYear);
            dateEndInterval.set(Calendar.DAY_OF_MONTH, dayOfMonth);
            setInitialDateTime();
        }
    };


    private DatePickerDialog.OnDateSetListener dateStart = new DatePickerDialog.OnDateSetListener() {
        public void onDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth) {
            dateStartInterval.set(Calendar.YEAR, year);
            dateStartInterval.set(Calendar.MONTH, monthOfYear);
            dateStartInterval.set(Calendar.DAY_OF_MONTH, dayOfMonth);
            setInitialDateTime();
        }
    };

    private void setInitialDateTime() {
        fieldDateStartInterval.setText(DateUtils.formatDateTime(view.getContext(),
                dateStartInterval.getTimeInMillis(),
                DateUtils.FORMAT_SHOW_DATE | DateUtils.FORMAT_SHOW_YEAR));
        fieldDateEndInterval.setText(DateUtils.formatDateTime(view.getContext(),
                dateEndInterval.getTimeInMillis(),
                DateUtils.FORMAT_SHOW_DATE | DateUtils.FORMAT_SHOW_YEAR));
    }

    public FilterData getFilter() {
        filter.Category = null;
        filter.Executor = null;
        filter.Owner = null;
        filter.Status = null;
        filter.DateInterval = new LinkedList<>();
        filter.DateInterval.add(dateStartInterval.getTimeInMillis());
        filter.DateInterval.add(dateEndInterval.getTimeInMillis());

        if (spinnerCategory != null) {
            filter.Category = spinnerCategory.getSelectedItems();
        }
        if (spinnerExecutor != null && spinnerExecutor.getVisibility() != View.GONE) {
            filter.Executor = spinnerExecutor.getSelectedItems();
        }
        if (spinnerOwner != null && spinnerOwner.getVisibility() != View.GONE) {
            filter.Owner = spinnerOwner.getSelectedItems();
        }
        if (spinnerStatus != null) {
            filter.Status = spinnerStatus.getSelectedItems();
        } else {
            filter.Status = new LinkedList<>();
            filter.Status.add(WorkflowStatus.InPlanned);
        }

        return filter;
    }

    @Override
    public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
        int planCode = Integer.valueOf(((TextView) view.findViewById(R.id.plan_entity_code)).getText().toString());

        PlanInformationFragment fragment = new PlanInformationFragment();
        fragment.setPlanCode(planCode);
        FragmentTransaction fragmentTransaction = getActivity().getSupportFragmentManager().beginTransaction();
        fragmentTransaction.replace(R.id.content_frame, fragment);
        fragmentTransaction.addToBackStack(null);
        fragmentTransaction.commit();
    }
}
