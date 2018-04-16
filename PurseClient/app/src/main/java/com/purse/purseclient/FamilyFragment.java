package com.purse.purseclient;


import android.app.ProgressDialog;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.purse.array_adapter.PlanAdapter;
import com.purse.array_adapter.UserAdapter;
import com.purse.entity.Plan;
import com.purse.entity.UserData;
import com.purse.services.RestService;

import java.util.Date;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.http.Part;

public class FamilyFragment extends Fragment {

    private View view;
    private ProgressDialog progress;
    private ListView userListView;

    public FamilyFragment() {

    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_family, container, false);

        userListView = (ListView) view.findViewById(R.id.user_list_view);

        progress = new ProgressDialog(view.getContext());

        loadDataUser();

        return view;
    }

    private void loadDataUser() {
        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();

        Call<List<UserData>> call = RestService.getService().usersList();

        call.enqueue(new Callback<List<UserData>>() {
            @Override
            public void onResponse(Call<List<UserData>> call, Response<List<UserData>> response) {
                progress.dismiss();
                if(response.isSuccessful()) {
                    List<UserData> users = response.body();

                    UserAdapter userAdapter = new UserAdapter(view.getContext(), R.layout.view_plan_entity, users);

                    userListView.setAdapter(userAdapter);

                }
                else
                    Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
            }

            @Override
            public void onFailure(Call<List<UserData>> call, Throwable t) {

            }
        });
    }


    private void showFamilyInformation() {
    }

}
