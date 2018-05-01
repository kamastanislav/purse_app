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
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.purse.array_adapter.PlanAdapter;
import com.purse.array_adapter.UserAdapter;
import com.purse.entity.Plan;
import com.purse.entity.UserData;
import com.purse.helper.Constants;
import com.purse.services.RestService;

import java.util.Date;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.http.Part;

public class FamilyFragment extends Fragment implements android.view.View.OnClickListener, AdapterView.OnItemClickListener {

    private View view;
    private ProgressDialog progress;
    private ListView userListView;
    private ListView searchUserListView;
    private LinearLayout familyCreate;
    private LinearLayout familyInfo;
    private LinearLayout familyListUser;

    public FamilyFragment() {

    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_family, container, false);

        userListView = (ListView) view.findViewById(R.id.user_list_view);
        userListView.setOnItemClickListener(this);
        searchUserListView = (ListView) view.findViewById(R.id.search_user_list_view);
        searchUserListView.setOnItemClickListener(this);
        Button create = (Button) view.findViewById(R.id.create_family);
        create.setOnClickListener(this);
        progress = new ProgressDialog(view.getContext());
        familyCreate = (LinearLayout) view.findViewById(R.id.family_create);
        familyInfo = (LinearLayout) view.findViewById(R.id.family_data);
        familyListUser = (LinearLayout) view.findViewById(R.id.family_list_user_data);

        TextView searchDataUser = (TextView) view.findViewById(R.id.search_data_user);
        searchDataUser.setOnClickListener(this);

        Button addUser = (Button) view.findViewById(R.id.add_user_btn);
        addUser.setOnClickListener(this);

        loadDataUser();

        return view;
    }

    private void loadDataUser() {
        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();

        Call<List<Boolean>> call = RestService.getService().infoFamily();
        call.enqueue(new Callback<List<Boolean>>() {
            @Override
            public void onResponse(Call<List<Boolean>> call, Response<List<Boolean>> response) {
                if (response.isSuccessful()) {
                    List<Boolean> info = response.body();
                    if (info.get(0)) {
                        familyInfo.setVisibility(info.get(1) ? View.VISIBLE : View.GONE);
                        familyListUser.setVisibility(View.VISIBLE);
                        loadUserList();

                    } else {
                        progress.dismiss();
                        familyCreate.setVisibility(View.VISIBLE);
                    }

                } else {
                    progress.dismiss();
                    Toast.makeText(view.getContext(), "No", Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void onFailure(Call<List<Boolean>> call, Throwable t) {

            }
        });

    }

    private void loadUserList() {
        Call<List<UserData>> call = RestService.getService().usersList();

        call.enqueue(new Callback<List<UserData>>() {
            @Override
            public void onResponse(Call<List<UserData>> call, Response<List<UserData>> response) {
                progress.dismiss();
                if (response.isSuccessful()) {
                    List<UserData> users = response.body();

                    UserAdapter userAdapter = new UserAdapter(view.getContext(), R.layout.view_plan_entity, users);

                    userListView.setAdapter(userAdapter);

                } else
                    Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
            }

            @Override
            public void onFailure(Call<List<UserData>> call, Throwable t) {

            }
        });
    }


    private void showFamilyInformation() {
    }

    @Override
    public void onClick(View v) {
        if (v == view.findViewById(R.id.create_family)) {
            Call<Integer> call = RestService.getService().createFamily();
            call.enqueue(new Callback<Integer>() {
                @Override
                public void onResponse(Call<Integer> call, Response<Integer> response) {
                    if (response.isSuccessful()) {
                        Constants.familyCode = response.body();
                    } else {
                        Toast.makeText(view.getContext(), "No", Toast.LENGTH_LONG).show();
                    }
                }

                @Override
                public void onFailure(Call<Integer> call, Throwable t) {

                }
            });
        } else if (v == view.findViewById(R.id.search_data_user)) {
            LinearLayout linear = (LinearLayout) view.findViewById(R.id.family_search_user);
            if (linear.getVisibility() == View.GONE) {
                linear.setVisibility(View.VISIBLE);
                familyListUser.setVisibility(View.GONE);
            } else {
                linear.setVisibility(View.GONE);
                familyListUser.setVisibility(View.VISIBLE);
            }
        } else if (v == view.findViewById(R.id.add_user_btn)) {
            searchUser();
        }
    }

    private void searchUser() {
        EditText userName = (EditText) view.findViewById(R.id.search_user_name);
        String name = userName.getText().toString();
        if (!name.equals("")) {
            progress.setTitle("Loading");
            progress.setMessage("Wait while loading...");
            progress.setCancelable(false);
            progress.show();

            Call<List<UserData>> call = RestService.getService().searchUsersList(name);

            call.enqueue(new Callback<List<UserData>>() {
                @Override
                public void onResponse(Call<List<UserData>> call, Response<List<UserData>> response) {
                    progress.dismiss();
                    if (response.isSuccessful()) {
                        List<UserData> users = response.body();

                        UserAdapter userAdapter = new UserAdapter(view.getContext(), R.layout.view_plan_entity, users);

                        searchUserListView.setAdapter(userAdapter);

                    } else
                        Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
                }

                @Override
                public void onFailure(Call<List<UserData>> call, Throwable t) {

                }
            });
        }
    }

    @Override
    public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
        int userCode = Integer.valueOf(((TextView) view.findViewById(R.id.user_entity_code)).getText().toString());

        UserFragment fragment = new UserFragment();
        fragment.setUserCode(userCode);
        FragmentTransaction fragmentTransaction = getActivity().getSupportFragmentManager().beginTransaction();
        fragmentTransaction.replace(R.id.content_frame, fragment);
        fragmentTransaction.addToBackStack(null);
        fragmentTransaction.commit();
    }
}
