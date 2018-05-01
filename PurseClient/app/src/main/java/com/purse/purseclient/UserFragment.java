package com.purse.purseclient;

import android.app.ProgressDialog;
import android.content.Context;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTransaction;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import com.purse.entity.UserData;
import com.purse.helper.Constants;
import com.purse.services.RestService;

import java.math.BigDecimal;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Date;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class UserFragment extends Fragment implements android.view.View.OnClickListener {

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
    private Button addUserInFamily;
    private Button addCash;
    private EditText fieldAddCash;
    private Button saveCash;

    private int userCode;

    public UserFragment() {
        userCode = Constants.DEFAULT_CODE;
    }

    public void setUserCode(int userCode) {
        this.userCode = userCode;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        view = inflater.inflate(R.layout.fragment_user, container, false);

        fieldNick = (TextView) view.findViewById(R.id.nickName_user);
        fieldEmail = (TextView) view.findViewById(R.id.email_user);
        fieldLastName = (TextView) view.findViewById(R.id.first_name_user);
        fieldFirstName = (TextView) view.findViewById(R.id.last_name_user);
        fieldPhone = (TextView) view.findViewById(R.id.phone_user);
        fieldCash = (TextView) view.findViewById(R.id.cash_user);
        fieldEmail = (TextView) view.findViewById(R.id.email_user);
        fieldCode = (TextView) view.findViewById(R.id.code_user);
        fieldBirthday = (TextView) view.findViewById(R.id.birthday_user);
        progress = new ProgressDialog(view.getContext());
        addCash = (Button) view.findViewById(R.id.add_cash);
        addCash.setOnClickListener(this);
        addUserInFamily = (Button) view.findViewById(R.id.add_user_in_family);
        addUserInFamily.setOnClickListener(this);

        saveCash = (Button) view.findViewById(R.id.add_cash_user);
        saveCash.setOnClickListener(this);

        fieldAddCash = (EditText) view.findViewById(R.id.add_new_user_cash);

        initializationData();

        return view;
    }

    private void initializationData() {

        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();
        Call<UserData> call;
        if (userCode == Constants.DEFAULT_CODE || userCode == Constants.userCode)
            call = RestService.getService().getSessionUser();
        else
            call = RestService.getService().getUser(userCode);

        call.enqueue(new Callback<UserData>() {
            @Override
            public void onResponse(Call<UserData> call, Response<UserData> response) {
                progress.dismiss();
                if (response.isSuccessful()) {
                    UserData userData = response.body();

                    if (userData != null) {
                        fieldCode.setText(String.valueOf(userData.Code));
                        fieldNick.setText(userData.NickName);

                        fieldFirstName.setText(userData.FirstName);
                        fieldLastName.setText(userData.LastName);

                        if (userCode == userData.Code || userData.FamilyCode == Constants.familyCode) {
                            fieldCash.setText(String.valueOf(userData.Cash));
                            fieldCash.setVisibility(View.VISIBLE);
                            fieldPhone.setText(userData.Phone);
                            fieldPhone.setVisibility(View.VISIBLE);
                            fieldEmail.setText(userData.Email);
                            fieldEmail.setVisibility(View.VISIBLE);
                        }

                        DateFormat dateFormat = new SimpleDateFormat("dd/MM/yyyy");
                        Date birthday = new Date(userData.Birthday);
                        fieldBirthday.setText(dateFormat.format(birthday));

                        if (userData.FamilyCode == Constants.familyCode) {
                            addCash.setVisibility(View.VISIBLE);
                        }

                        if (userData.FamilyCode == Constants.DEFAULT_CODE && Constants.userCode != userData.Code) {
                            addUserInFamily.setVisibility(View.VISIBLE);
                        }
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

    @Override
    public void onClick(View v) {
        if (v == view.findViewById(R.id.add_cash)) {
            fieldAddCash.setVisibility(View.VISIBLE);
            saveCash.setVisibility(View.VISIBLE);
            addCash.setVisibility(View.GONE);
        } else if (v == view.findViewById(R.id.add_user_in_family)) {
            addUserInFamily();
        } else if (v == view.findViewById(R.id.add_cash_user)) {
            addCash();
        }
    }

    private void addCash() {
        fieldAddCash.setVisibility(View.GONE);
        saveCash.setVisibility(View.GONE);
        addCash.setVisibility(View.VISIBLE);

        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();

        BigDecimal budget = BigDecimal.valueOf(Double.valueOf(fieldAddCash.getText().toString()));

        Call<Boolean> call = RestService.getService().budgetReplenishment(budget);
        call.enqueue(new Callback<Boolean>() {
            @Override
            public void onResponse(Call<Boolean> call, Response<Boolean> response) {
                progress.dismiss();
                if (response.isSuccessful()) {
                    Boolean isGood = response.body();
                    if (isGood != null && isGood)
                        initializationData();
                } else {
                    Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void onFailure(Call<Boolean> call, Throwable t) {

            }
        });
    }

    private void addUserInFamily() {
        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();
        Call<Boolean> call = RestService.getService().addUserInFamily(userCode);

        call.enqueue(new Callback<Boolean>() {
            @Override
            public void onResponse(Call<Boolean> call, Response<Boolean> response) {
                progress.dismiss();
                if(response.isSuccessful()){
                    FamilyFragment fragment = new FamilyFragment();
                    FragmentTransaction fragmentTransaction = getActivity().getSupportFragmentManager().beginTransaction();
                    fragmentTransaction.replace(R.id.content_frame, fragment);
                    fragmentTransaction.addToBackStack(null);
                    fragmentTransaction.commit();
                }
            }

            @Override
            public void onFailure(Call<Boolean> call, Throwable t) {

            }
        });
    }
}
