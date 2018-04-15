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

import java.util.Date;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class UserFragment extends Fragment {

    private TextView fieldNick;
    private TextView fieldEmail;
    private TextView fieldPhone;
    private TextView fieldFirstName;
    private TextView fieldLastName;
    private TextView fieldCash;
    private TextView fieldCode;
    private TextView fieldBirthday;
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
        fieldEmail = (TextView)view.findViewById(R.id.email_user);
        fieldLastName = (TextView)view.findViewById(R.id.first_name_user);
        fieldFirstName = (TextView)view.findViewById(R.id.last_name_user);
        fieldPhone = (TextView)view.findViewById(R.id.phone_user);
        fieldCash = (TextView)view.findViewById(R.id.cash_user);
        fieldEmail = (TextView)view.findViewById(R.id.email_user);
        fieldCode = (TextView)view.findViewById(R.id.code_user);
        fieldBirthday = (TextView)view.findViewById(R.id.birthday_user);
        progress = new ProgressDialog(view.getContext());

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
                        fieldCash.setText(String.valueOf(userData.Cash));
                        fieldFirstName.setText(userData.FirstName);
                        fieldLastName.setText(userData.LastName);
                        fieldPhone.setText(userData.Phone);
                        fieldEmail.setText(userData.Email);
                        Date birthday = new Date(userData.Birthday);
                        fieldBirthday.setText(String.valueOf(birthday));
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
