package com.purse.purseclient;


import android.app.ProgressDialog;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTransaction;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;
import android.widget.Toast;

import com.purse.entity.Flight;
import com.purse.helper.Constants;
import com.purse.services.RestService;

import java.util.Date;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class FlightInformationFragment extends Fragment {
    private int flightCode;
    private String namePlan;
    private int planCode;

    private ProgressDialog progress;
    private View view;
    private TextView fieldPlanName;
    private TextView fieldPlannedBudget;
    private TextView fieldComment;
    private TextView fieldOwner;
    private TextView fieldDate;

    public FlightInformationFragment() {

    }

    public void setInformation(int flightCode, int planCode, String namePlan)
    {
        this.flightCode = flightCode;
        this.namePlan = namePlan;
        this.planCode = planCode;
    }
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_flight_information, container, false);

        progress = new ProgressDialog(view.getContext());
        fieldPlanName = (TextView)view.findViewById(R.id.name_plan_flight_info);
        fieldPlanName.setText(namePlan);
        fieldPlannedBudget = (TextView)view.findViewById(R.id.planned_budget_flight_info);
        fieldComment = (TextView)view.findViewById(R.id.comment_flight_info);
        fieldOwner = (TextView)view.findViewById(R.id.owner_flight_info);
        fieldOwner.setText(Constants.userName);
        fieldDate = (TextView)view.findViewById(R.id.create_flight_info);
        
        loadDataFlight();
        
        return view;
    }

    private void loadDataFlight() {
        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();

        Call<Flight> call = RestService.getService().flightPlan(flightCode);

        call.enqueue(new Callback<Flight>() {
            @Override
            public void onResponse(Call<Flight> call, Response<Flight> response) {
                progress.dismiss();
                if (response.isSuccessful()) {
                    Flight flight = response.body();
                    if(flight != null)
                    {
                        fieldComment.setText(flight.Comment);
                        Date date = new Date(flight.DateCreate);
                        fieldDate.setText(date.toString());
                        fieldPlannedBudget.setText(flight.PlannedBudget.toString());
                    }

                } else {
                    Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void onFailure(Call<Flight> call, Throwable t) {

            }
        });

    }

}
