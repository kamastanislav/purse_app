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
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.purse.entity.Flight;
import com.purse.helper.Constants;
import com.purse.helper.WorkflowStatus;
import com.purse.services.RestService;

import java.math.BigDecimal;
import java.util.Date;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class FlightInformationFragment extends Fragment implements android.view.View.OnClickListener {
    private int flightCode;
    private String namePlan;
    private int planCode;
    private boolean isTimeActualize;
    private boolean isDeletedFlight;

    private ProgressDialog progress;
    private View view;
    private TextView fieldPlannedBudget;
    private TextView fieldComment;
    private TextView fieldActualBudget;
    private TextView fieldDate;

    private LinearLayout approveFlightData;
    private LinearLayout flightData;

    private Button actualizeBtn;
    private Button deletedBnt;
    private Button editorBnt;

    public FlightInformationFragment() {

    }

    public void setInformation(int flightCode, String namePlan, boolean isTimeActualize, boolean isDeletedFlight) {
        this.flightCode = flightCode;
        this.namePlan = namePlan;
        this.isTimeActualize = isTimeActualize;
        this.isDeletedFlight = isDeletedFlight;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_flight_information, container, false);

        progress = new ProgressDialog(view.getContext());
        TextView fieldPlanName = (TextView) view.findViewById(R.id.name_plan_flight_info);
        fieldPlanName.setText(namePlan);
        fieldPlannedBudget = (TextView) view.findViewById(R.id.planned_budget_flight_info);
        fieldComment = (TextView) view.findViewById(R.id.comment_flight_info);
        TextView fieldOwner = (TextView) view.findViewById(R.id.owner_flight_info);
        fieldOwner.setText(Constants.userName);
        fieldDate = (TextView) view.findViewById(R.id.create_flight_info);
        fieldActualBudget = (TextView) view.findViewById(R.id.actual_budget_flight_info);

        actualizeBtn = (Button) view.findViewById(R.id.actualize_flight_btn);
        actualizeBtn.setOnClickListener(this);

        deletedBnt = (Button) view.findViewById(R.id.delete_flight_btn);
        deletedBnt.setOnClickListener(this);

        editorBnt = (Button) view.findViewById(R.id.editor_flight_btn);
        editorBnt.setOnClickListener(this);

        Button saveActualBudget = (Button) view.findViewById(R.id.save_actual_budget_flight);
        saveActualBudget.setOnClickListener(this);

        approveFlightData = (LinearLayout) view.findViewById(R.id.approve_flight_data);
        flightData = (LinearLayout) view.findViewById(R.id.flight_data);

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
                    if (flight != null) {
                        fieldComment.setText(flight.Comment);
                        Date date = new Date(flight.DateCreate);
                        fieldDate.setText(date.toString());

                        if (flight.Status == WorkflowStatus.Approved) {
                            fieldPlannedBudget.setText(String.valueOf(flight.PlannedBudget.doubleValue()));
                            fieldActualBudget.setVisibility(View.GONE);
                            actualizeBtn.setVisibility(View.GONE);
                            deletedBnt.setVisibility(View.GONE);
                            editorBnt.setVisibility(View.GONE);
                        } else {
                            fieldActualBudget.setText(String.valueOf(flight.ActualBudget.doubleValue()));
                            fieldPlannedBudget.setVisibility(View.GONE);
                            deletedBnt.setVisibility(isDeletedFlight || Constants.userCode == flight.OwnerCode ? View.VISIBLE : View.GONE);
                            actualizeBtn.setVisibility(isTimeActualize ? View.VISIBLE : View.GONE);
                        }

                        planCode = flight.PlanCode;
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
        if (v == view.findViewById(R.id.actualize_flight_btn)) {
            boolean isGone = approveFlightData.getVisibility() == View.GONE;
            if (isGone) {
                approveFlightData.setVisibility(View.VISIBLE);
                flightData.setVisibility(View.GONE);
            } else {
                approveFlightData.setVisibility(View.GONE);
                flightData.setVisibility(View.VISIBLE);
            }
        } else if (v == view.findViewById(R.id.editor_flight_btn)) {
            loadEditorFragment();
        } else if (v == view.findViewById(R.id.delete_flight_btn)) {
            deleteFlight();
        } else if (v == view.findViewById(R.id.save_actual_budget_flight)) {
            approveFlight();
        }
    }

    private void loadEditorFragment() {
        FlightEditorFragment fragment = new FlightEditorFragment();
        fragment.setInfoPlan(planCode, namePlan, flightCode);
        FragmentTransaction fragmentTransaction = getActivity().getSupportFragmentManager().beginTransaction();
        fragmentTransaction.replace(R.id.content_frame, fragment);
        fragmentTransaction.addToBackStack(null);
        fragmentTransaction.commit();
    }

    private void deleteFlight() {
        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();

        Call<Boolean> call = RestService.getService().deleteFlightPlan(flightCode);

        call.enqueue(new Callback<Boolean>() {
            @Override
            public void onResponse(Call<Boolean> call, Response<Boolean> response) {
                progress.dismiss();
                if (response.isSuccessful()) {
                    Boolean result = response.body();
                    if (result != null && result) {
                        callPlanInformation();
                    }

                }
            }

            @Override
            public void onFailure(Call<Boolean> call, Throwable t) {

            }
        });
    }

    private void approveFlight() {

        EditText actualBudgetFlight = (EditText) view.findViewById(R.id.actual_budget_flight);

        String actualBudget = actualBudgetFlight.getText().toString();

        if (TextUtils.isEmpty(actualBudget)) {
            actualBudgetFlight.setError("Actual budget is required!");
            return;
        }

        BigDecimal budget = BigDecimal.valueOf(Double.valueOf(actualBudget));

        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();

        Call<Boolean> call = RestService.getService().approveFlightPlan(flightCode, budget);

        call.enqueue(new Callback<Boolean>() {
            @Override
            public void onResponse(Call<Boolean> call, Response<Boolean> response) {
                progress.dismiss();
                if (response.isSuccessful()) {
                    Boolean result = response.body();
                    if (result != null && result) {
                        callPlanInformation();
                    }

                }
            }

            @Override
            public void onFailure(Call<Boolean> call, Throwable t) {

            }
        });
    }

    private void callPlanInformation() {
        PlanInformationFragment fragment = new PlanInformationFragment();
        fragment.setPlanCode(planCode);
        FragmentTransaction fragmentTransaction = getActivity().getSupportFragmentManager().beginTransaction();
        fragmentTransaction.replace(R.id.content_frame, fragment);
        fragmentTransaction.addToBackStack(null);
        fragmentTransaction.commit();
    }
}
