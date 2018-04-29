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

public class FamilyFragment extends Fragment implements android.view.View.OnClickListener {

    private View view;
    private ProgressDialog progress;
    private ListView userListView;

    public FamilyFragment() {

    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_family, container, false);

        // userListView = (ListView) view.findViewById(R.id.user_list_view);
        Button create = (Button) view.findViewById(R.id.create_family);
        create.setOnClickListener(this);
        progress = new ProgressDialog(view.getContext());

        loadDataUser();

        return view;
    }

    private void loadDataUser() {
       /* progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();*/

        Call<Boolean> callHaveFamily = RestService.getService().havingFamily();
        callHaveFamily.enqueue(new Callback<Boolean>() {
            @Override
            public void onResponse(Call<Boolean> call, Response<Boolean> response) {
                if (response.isSuccessful()) {
                    Toast.makeText(view.getContext(), "callHaveFamily " + response.body(), Toast.LENGTH_LONG).show();
                } else {
                    Toast.makeText(view.getContext(), "No", Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void onFailure(Call<Boolean> call, Throwable t) {

            }
        });

        Call<Boolean> callIsAdminFamily = RestService.getService().isAdminFamily();
        callIsAdminFamily.enqueue(new Callback<Boolean>() {
            @Override
            public void onResponse(Call<Boolean> call, Response<Boolean> response) {
                if (response.isSuccessful()) {
                    Toast.makeText(view.getContext(), "callIsAdminFamily " + response.body(), Toast.LENGTH_LONG).show();
                } else {
                    Toast.makeText(view.getContext(), "No", Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void onFailure(Call<Boolean> call, Throwable t) {

            }
        });

       /* Call<List<UserData>> call = RestService.getService().usersList();

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
        });*/
    }


    private void showFamilyInformation() {
    }

    @Override
    public void onClick(View v) {
        if (v == view.findViewById(R.id.create_family)) {
            Call<Boolean> call = RestService.getService().createFamily();
            call.enqueue(new Callback<Boolean>() {
                @Override
                public void onResponse(Call<Boolean> call, Response<Boolean> response) {
                    if (response.isSuccessful()) {
                        Toast.makeText(view.getContext(), "createFamily " + response.body(), Toast.LENGTH_LONG).show();
                    } else {
                        Toast.makeText(view.getContext(), "No", Toast.LENGTH_LONG).show();
                    }
                }

                @Override
                public void onFailure(Call<Boolean> call, Throwable t) {

                }
            });
        }
    }
}
