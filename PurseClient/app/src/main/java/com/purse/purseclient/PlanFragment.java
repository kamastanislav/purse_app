package com.purse.purseclient;


import android.app.ProgressDialog;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.purse.array_adapter.PlanAdapter;
import com.purse.entity.Plan;
import com.purse.services.RestService;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class PlanFragment extends Fragment {
    private FragmentManager fragmentManager;
    private ListView planListView;
    private ProgressDialog progress;
    private View view;

    public PlanFragment() {

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

        planListView = (ListView) view.findViewById(R.id.plan_list_view);

        planListView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
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
        });

        initialPlanListView();

        return view;
    }

    private void initialPlanListView() {
        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();

        Call<List<Plan>> call = RestService.getService().allPlanOwner(1);
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

}
