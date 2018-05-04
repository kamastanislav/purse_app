package com.purse.purseclient;

import android.app.DatePickerDialog;
import android.app.ProgressDialog;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTransaction;
import android.text.format.DateUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import com.purse.entity.CategoryService;
import com.purse.entity.Plan;
import com.purse.entity.Service;
import com.purse.entity.UserData;
import com.purse.helper.Constants;
import com.purse.helper.FilterPlan;
import com.purse.services.RestService;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class PlanEditorFragment extends Fragment {
    private Calendar dateStartPlan = Calendar.getInstance();
    private Calendar dateEndPlan = Calendar.getInstance();
    private TextView fieldNamePlan;
    private TextView fieldDateStartPlan;
    private TextView fieldDateEndPlan;
    private TextView fieldOwnerPlan;
    private View view;
    private ProgressDialog progress;
    private FilterPlan filterPlan;
    private Spinner categoryService;
    private Spinner service;
    private Spinner executor;

    private Button savePlan;

    private int indexExecutor = 0;
    private int indexCategoryService = 0;
    private int indexService = 0;

    private int planCode;
    private Plan plan;
    private boolean isTemplate;

    public PlanEditorFragment() {
        planCode = Constants.DEFAULT_CODE;
    }

    public void setPlanCode(int planCode, boolean isTemplate) {
        this.planCode = planCode;
        this.isTemplate = isTemplate;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_plan_editor, container, false);
        fieldDateStartPlan = (TextView) view.findViewById(R.id.start_date_plan);
        fieldDateEndPlan = (TextView) view.findViewById(R.id.end_date_plan);
        fieldOwnerPlan = (TextView) view.findViewById(R.id.owner_plan);
        categoryService = (Spinner) view.findViewById(R.id.category_plan);
        service = (Spinner) view.findViewById(R.id.service_plan);
        executor = (Spinner) view.findViewById(R.id.executor_plan);
        fieldNamePlan = (TextView) view.findViewById(R.id.name_plan);
        fieldDateEndPlan.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                new DatePickerDialog(view.getContext(), dateEnd,
                        dateEndPlan.get(Calendar.YEAR),
                        dateEndPlan.get(Calendar.MONTH),
                        dateEndPlan.get(Calendar.DAY_OF_MONTH))
                        .show();
            }

        });
        fieldDateStartPlan.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                new DatePickerDialog(view.getContext(), dateStart,
                        dateStartPlan.get(Calendar.YEAR),
                        dateStartPlan.get(Calendar.MONTH),
                        dateStartPlan.get(Calendar.DAY_OF_MONTH))
                        .show();
            }

        });

        categoryService.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {

            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                indexCategoryService = position;
                indexService = 0;
                List<String> listService = new ArrayList<String>();
                for (Service serv : filterPlan.CategoryServices.get(indexCategoryService).Services)
                    listService.add(serv.Name);
                ArrayAdapter<String> dataAdapterService = new ArrayAdapter<String>(view.getContext(),
                        android.R.layout.simple_spinner_item, listService);
                dataAdapterService.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                service.setAdapter(dataAdapterService);
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });
        executor.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                indexExecutor = position;
                updatePlanName();
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });
        service.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                indexService = position;
                updatePlanName();
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });

        if (isTemplate) {
            service.setVisibility(View.GONE);
            categoryService.setVisibility(View.GONE);
        }

        savePlan = (Button) view.findViewById(R.id.save_plan);

        savePlan.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                savePlanAction();
            }
        });

        progress = new ProgressDialog(view.getContext());
        loadFilterPlan();
        try {
            Thread.sleep(1000);
        } catch (InterruptedException e) {
            e.printStackTrace();

        }
        if (planCode != Constants.DEFAULT_CODE)
            loadPlanData();

        if (isTemplate || planCode == Constants.DEFAULT_CODE)
            setInitialDateTime();

        return view;
    }

    private void loadPlanData() {
        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();

        Call<Plan> call = RestService.getService().plan(planCode);
        call.enqueue(new Callback<Plan>() {
            @Override
            public void onResponse(Call<Plan> call, Response<Plan> response) {
                progress.dismiss();
                if (response.isSuccessful()) {
                    plan = response.body();
                    if (plan == null)
                        return;

                    if (!isTemplate) {
                        dateStartPlan.setTimeInMillis(plan.StartDate);
                        dateEndPlan.setTimeInMillis(plan.EndDate);
                        setInitialDateTime();
                    }
                    fieldOwnerPlan.setText(Constants.userName);
                    fieldNamePlan.setText(plan.Name);

                    if (!filterPlan.Executors.isEmpty()) {
                        for (int i = 0; i < filterPlan.Executors.size(); i++) {
                            UserData user = filterPlan.Executors.get(i);
                            if (user.Code == plan.ExecutorCode) {
                                indexExecutor = i;
                                break;
                            }
                        }
                        executor.setSelection(indexExecutor);
                    }

                    for (int i = 0; i < filterPlan.CategoryServices.size(); i++) {
                        CategoryService categoryService = filterPlan.CategoryServices.get(i);
                        if (categoryService.Code == plan.CategoryCode) {
                            indexCategoryService = i;
                            break;
                        }
                    }
                    categoryService.setSelection(indexCategoryService);

                    for (int i = 0; i < filterPlan.CategoryServices.get(indexCategoryService).Services.size(); i++) {
                        Service service = filterPlan.CategoryServices.get(indexCategoryService).Services.get(i);
                        if (service.Code == plan.ServiceCode) {
                            indexService = i;
                            break;
                        }
                    }
                    service.setSelection(indexService);

                } else
                    Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();

            }

            @Override
            public void onFailure(Call<Plan> call, Throwable t) {

            }
        });
    }

    private void updatePlanName() {
        String name = "";
        try {
            name = filterPlan.CategoryServices.get(indexCategoryService).Name;
            name = String.format("%1$s %2$s", name, filterPlan.CategoryServices.get(indexCategoryService).Services.get(indexService).Name);

            DateFormat dateFormat = new SimpleDateFormat("dd.MM");

            name = String.format("%1$s %2$s/%3$s", name, dateFormat.format(new Date(dateStartPlan.getTimeInMillis())), dateFormat.format(new Date(dateEndPlan.getTimeInMillis())));
        } catch (Exception ex) {
            Log.e("exception", ex.getMessage());
        }
        fieldNamePlan.setText(name);
    }

    private void savePlanAction() {
        plan = plan == null ? new Plan() : plan;

        plan.Name = fieldNamePlan.getText().toString();
        CategoryService sc = filterPlan.CategoryServices.get(indexCategoryService);
        plan.CategoryCode = sc.Code;
        plan.ServiceCode = sc.Services.get(indexService).Code;
        if (executor.getVisibility() != View.GONE)
            plan.ExecutorCode = filterPlan.Executors.get(indexExecutor).Code;
        else
            plan.ExecutorCode = Constants.userCode;
        plan.StartDate = dateStartPlan.getTimeInMillis();
        plan.EndDate = dateEndPlan.getTimeInMillis();

        if (planCode == Constants.DEFAULT_CODE)
            createPlan();
        else
            updatePlan();

    }

    private void updatePlan() {
        Call<Boolean> call = RestService.getService().updatePlan(planCode, plan);
        loadCall(call);
    }

    private void loadCall(Call<Boolean> call) {
        call.enqueue(new Callback<Boolean>() {
            @Override
            public void onResponse(Call<Boolean> call, Response<Boolean> response) {
                progress.dismiss();
                if (response.isSuccessful()) {

                    Boolean good = response.body();
                    if (good != null) {
                        replaceFragment();

                    } else {
                        Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
                    }
                } else {
                    Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void onFailure(Call<Boolean> call, Throwable t) {

            }
        });
    }


    private void createPlan() {
        Call<Boolean> call = RestService.getService().savePlan(plan);
        loadCall(call);
    }

    private void replaceFragment() {
        PlanFragment fragment = new PlanFragment();
        FragmentTransaction fragmentTransaction = getActivity().getSupportFragmentManager().beginTransaction();
        fragmentTransaction.replace(R.id.content_frame, fragment);
        fragmentTransaction.addToBackStack(null);
        fragmentTransaction.commit();
    }

    private void loadFilterPlan() {
        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();

        Call<FilterPlan> call = RestService.getService().setFilterPlan();

        call.enqueue(new Callback<FilterPlan>() {
            @Override
            public void onResponse(Call<FilterPlan> call, Response<FilterPlan> response) {
                progress.dismiss();
                if (response.isSuccessful()) {
                    FilterPlan filterPlanUser = response.body();

                    if (filterPlanUser != null) {

                        filterPlan = filterPlanUser;

                        List<String> listCS = new ArrayList<String>();
                        for (CategoryService category : filterPlan.CategoryServices)
                            listCS.add(category.Name);
                        ArrayAdapter<String> dataAdapterCS = new ArrayAdapter<String>(view.getContext(),
                                android.R.layout.simple_spinner_item, listCS);
                        dataAdapterCS.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                        categoryService.setAdapter(dataAdapterCS);

                        if (!filterPlan.Executors.isEmpty()) {
                            List<String> listExecutor = new ArrayList<String>();
                            for (UserData user : filterPlan.Executors)
                                listExecutor.add(String.format("%1$s %2$s (%3$s)", user.FirstName, user.LastName, user.NickName));
                            ArrayAdapter<String> dataAdapterExecutor = new ArrayAdapter<String>(view.getContext(),
                                    android.R.layout.simple_spinner_item, listExecutor);
                            dataAdapterExecutor.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                            executor.setAdapter(dataAdapterExecutor);
                        } else
                            executor.setVisibility(View.GONE);

                        List<String> listService = new ArrayList<String>();
                        for (Service serv : filterPlan.CategoryServices.get(indexCategoryService).Services)
                            listService.add(serv.Name);
                        ArrayAdapter<String> dataAdapterService = new ArrayAdapter<String>(view.getContext(),
                                android.R.layout.simple_spinner_item, listService);
                        dataAdapterService.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                        service.setAdapter(dataAdapterService);
                        updatePlanName();

                        Constants.userName = filterPlan.OwnerUser;
                        Constants.familyCode = filterPlan.FamilyCode;

                        fieldOwnerPlan.setText(filterPlan.OwnerUser);
                    } else {
                        Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
                    }
                } else {
                    Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void onFailure(Call<FilterPlan> call, Throwable t) {

            }
        });

    }

    private void setInitialDateTime() {
        fieldDateStartPlan.setText(DateUtils.formatDateTime(view.getContext(),
                dateStartPlan.getTimeInMillis(),
                DateUtils.FORMAT_SHOW_DATE | DateUtils.FORMAT_SHOW_YEAR));
        fieldDateEndPlan.setText(DateUtils.formatDateTime(view.getContext(),
                dateEndPlan.getTimeInMillis(),
                DateUtils.FORMAT_SHOW_DATE | DateUtils.FORMAT_SHOW_YEAR));
        updatePlanName();
    }

    private DatePickerDialog.OnDateSetListener dateEnd = new DatePickerDialog.OnDateSetListener() {
        public void onDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth) {
            dateEndPlan.set(Calendar.YEAR, year);
            dateEndPlan.set(Calendar.MONTH, monthOfYear);
            dateEndPlan.set(Calendar.DAY_OF_MONTH, dayOfMonth);
            setInitialDateTime();
        }
    };

    private DatePickerDialog.OnDateSetListener dateStart = new DatePickerDialog.OnDateSetListener() {
        public void onDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth) {
            dateStartPlan.set(Calendar.YEAR, year);
            dateStartPlan.set(Calendar.MONTH, monthOfYear);
            dateStartPlan.set(Calendar.DAY_OF_MONTH, dayOfMonth);
            setInitialDateTime();
        }
    };

}
