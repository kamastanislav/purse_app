package com.purse.purseclient;


import android.app.ProgressDialog;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTransaction;
import android.text.TextUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.purse.array_adapter.PlanAdapter;
import com.purse.array_adapter.PlanFlightAdapter;
import com.purse.entity.Flight;
import com.purse.entity.Plan;
import com.purse.entity.UserData;
import com.purse.services.RestService;

import java.math.BigDecimal;
import java.util.Date;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;


/**
 * A simple {@link Fragment} subclass.
 */
public class PlanInformationFragment extends Fragment implements android.view.View.OnClickListener {
    private int planCode;
    private View view;
    private ProgressDialog progress;
    private TextView fieldCode;
    private TextView fieldName;
    private TextView fieldPlannedBudget;
    private TextView fieldStartDate;
    private TextView fieldEndDate;
    private ListView flightsPlanListView;

    private Button addFlight;

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
        progress = new ProgressDialog(view.getContext());
        addFlight = (Button)view.findViewById(R.id.add_flight_plan_btn);
        addFlight.setOnClickListener(this);
        flightsPlanListView = (ListView) view.findViewById(R.id.flights_plan_list_view);

        initializationPlanData();

        return view;
    }
    private void initializationPlanData() {

        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();

        Call<Plan> call_plan = RestService.getService().plan(planCode);

        call_plan.enqueue(new Callback<Plan>(){
            @Override
            public void onResponse(Call<Plan> call, Response<Plan> response) {
                progress.dismiss();
                if (response.isSuccessful()) {
                    Plan plan = response.body();

                    if (plan != null) {
                        fieldCode.setText(String.valueOf(plan.Code));
                        fieldName.setText(plan.Name);
                        fieldPlannedBudget.setText(String.valueOf(plan.PlannedBudget));

                        Date startDate = new Date(plan.StartDate);
                        fieldStartDate.setText(String.valueOf(startDate));
                        Date endDate = new Date(plan.EndDate);
                        fieldEndDate.setText(String.valueOf(endDate));
                        progress.dismiss();


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

        Call<List<Flight>> call_flights = RestService.getService().flightsPlan(planCode);

        call_flights.enqueue(new Callback<List<Flight>>() {
            @Override
            public void onResponse(Call<List<Flight>> call, Response<List<Flight>> response) {
                if (response.isSuccessful()) {
                    List<Flight> flights = response.body();

                    PlanFlightAdapter planFlightAdapter = new PlanFlightAdapter(view.getContext(), R.layout.view_flights_entity, flights);

                    flightsPlanListView.setAdapter(planFlightAdapter);

                    flightsPlanListView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
                        @Override
                        public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                            int flightCode = Integer.valueOf(((TextView) view.findViewById(R.id.flight_entity_code)).getText().toString());

                            FlightInformationFragment fragment = new FlightInformationFragment();
                            fragment.setInformation(flightCode, planCode, fieldName.getText().toString());
                            FragmentTransaction fragmentTransaction = getActivity().getSupportFragmentManager().beginTransaction();
                            fragmentTransaction.replace(R.id.content_frame, fragment);
                            fragmentTransaction.addToBackStack(null);
                            fragmentTransaction.commit();
                        }
                    });

                } else {
                    Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void onFailure(Call<List<Flight>> call, Throwable t) {

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

        }
    }
}
