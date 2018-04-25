package com.purse.purseclient;


import android.app.ProgressDialog;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTransaction;
import android.text.TextUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.purse.array_adapter.PlanAdapter;
import com.purse.entity.Flight;
import com.purse.entity.Plan;
import com.purse.helper.Constants;
import com.purse.helper.WorkflowStatus;
import com.purse.services.RestService;

import java.math.BigDecimal;
import java.util.Date;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class FlightEditorFragment extends Fragment implements android.view.View.OnClickListener {
    private View view;
    private ProgressDialog progress;
    private TextView fieldPlanName;
    private TextView fieldPlannedBudget;
    private TextView fieldComment;
    private TextView fieldOwner;

    private int code;
    private int planCode;
    private String name;

    private Flight flight;

    public FlightEditorFragment() {
        // Required empty public constructor
    }

    public void setInfoPlan(int planCode, String name) {
        this.planCode = planCode;
        this.name = name;
        this.code = Constants.DEFAULT_CODE;
    }

    public void setInfoPlan(int planCode, String name, int code) {
        this.planCode = planCode;
        this.name = name;
        this.code = code;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        flight  = null;
        view = inflater.inflate(R.layout.fragment_flight_editor, container, false);
        Button saveBtn = (Button) view.findViewById(R.id.save_flight);
        progress = new ProgressDialog(view.getContext());
        fieldPlanName = (TextView) view.findViewById(R.id.name_plan_flight);
        fieldPlanName.setText(name);
        fieldPlannedBudget = (TextView) view.findViewById(R.id.planned_budget_flight);
        fieldComment = (TextView) view.findViewById(R.id.comment_flight);

        saveBtn.setOnClickListener(this);


        if (code != Constants.DEFAULT_CODE) {
            initFlight();
        } else {
            fieldOwner = (TextView) view.findViewById(R.id.owner_flight);
            fieldOwner.setText(Constants.userName);
        }
        return view;
    }

    private void initFlight() {
        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();
        Call<Flight> call = RestService.getService().flightPlan(this.code);

        call.enqueue(new Callback<Flight>() {
            @Override
            public void onResponse(Call<Flight> call, Response<Flight> response) {
                progress.dismiss();
                if (response.isSuccessful()) {
                    flight = response.body();
                    if (flight != null) {
                        fieldComment.setText(flight.Comment);

                        fieldPlannedBudget.setText(String.valueOf(flight.PlannedBudget.doubleValue()));

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

    @Override
    public void onClick(View v) {
        if (v == view.findViewById(R.id.save_flight)) {

            String comment = fieldComment.getText().toString();
            String plannedBudget = fieldPlannedBudget.getText().toString();

            if (TextUtils.isEmpty(comment)) {
                fieldComment.setError("Comment is required!");
                return;
            }
            if (TextUtils.isEmpty(plannedBudget)) {
                fieldPlannedBudget.setError("Planned budget is required!");
                return;
            }

            progress.setTitle("Loading");
            progress.setMessage("Wait while loading...");
            progress.setCancelable(false);
            progress.show();

            flight = flight == null ? new Flight() : flight;
            flight.PlanCode = planCode;
            flight.Comment = comment;
            flight.PlannedBudget = BigDecimal.valueOf(Double.valueOf(plannedBudget));
            flight.ActualBudget = new BigDecimal(0);

            if (code == Constants.DEFAULT_CODE) {
                createFlight();
            } else {
                updateFlight();
            }
        }

    }

    private void updateFlight() {
        Call<Boolean> call = RestService.getService().updateFlightPlan(flight.Code, flight.PlannedBudget, flight.Comment);
        call.enqueue(new Callback<Boolean>() {
            @Override
            public void onResponse(Call<Boolean> call, Response<Boolean> response) {
                progress.dismiss();
                if (response.isSuccessful()) {
                    loadPlanInfo();
                } else {
                    Toast.makeText(view.getContext(), response.message(), Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void onFailure(Call<Boolean> call, Throwable t) {

            }
        });
    }

    private void loadPlanInfo() {
        PlanInformationFragment fragment = new PlanInformationFragment();
        fragment.setPlanCode(planCode);
        FragmentTransaction fragmentTransaction = getActivity().getSupportFragmentManager().beginTransaction();
        fragmentTransaction.replace(R.id.content_frame, fragment);
        fragmentTransaction.addToBackStack(null);
        fragmentTransaction.commit();
    }

    private void createFlight() {
        Call<Boolean> call = RestService.getService().createFlight(flight.PlanCode, flight.PlannedBudget, flight.Comment);
        call.enqueue(new Callback<Boolean>() {
            @Override
            public void onResponse(Call<Boolean> call, Response<Boolean> response) {
                progress.dismiss();
                if (response.isSuccessful()) {
                    loadPlanInfo();
                } else {
                    Toast.makeText(view.getContext(), response.message(), Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void onFailure(Call<Boolean> call, Throwable t) {
                progress.dismiss();
                Toast.makeText(view.getContext(), t.toString(), Toast.LENGTH_LONG).show();
            }
        });
    }
}
