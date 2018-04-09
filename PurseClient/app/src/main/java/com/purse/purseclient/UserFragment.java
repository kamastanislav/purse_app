package com.purse.purseclient;

import android.app.ProgressDialog;
import android.content.Context;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;
import android.widget.Toast;

import com.purse.entity.UserData;
import com.purse.services.RestService;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class UserFragment extends Fragment {

    private TextView fieldNick;
    private TextView fieldPassword;
    private TextView fieldCode;
    private ProgressDialog progress;
    private View view;
    public UserFragment() {

    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        view = inflater.inflate(R.layout.fragment_user, container, false);

        fieldNick = (TextView)view.findViewById(R.id.nickName_user);
        fieldPassword = (TextView)view.findViewById(R.id.password_user);

        fieldCode = (TextView)view.findViewById(R.id.code_user);
        progress = new ProgressDialog(view.getContext());

        //   restAdapter = new RestService();

        initializationData();

        return view;
    }

    private void initializationData() {

        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();

        Call<UserData> call = RestService.getService().getSessionUser();

        call.enqueue(new Callback<UserData>() {
            @Override
            public void onResponse(Call<UserData> call, Response<UserData> response) {
                progress.dismiss();
                if (response.isSuccessful()) {
                    UserData userData = response.body();

                    if (userData != null) {
                        fieldCode.setText(String.valueOf(userData.Code));
                        fieldNick.setText(userData.NickName);
                        fieldPassword.setText(userData.Password);
                        progress.dismiss();


                    } else {
                        Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
                    }
                } else {
                    Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void onFailure(Call<UserData> call, Throwable t) {

            }
        });
    }

}
