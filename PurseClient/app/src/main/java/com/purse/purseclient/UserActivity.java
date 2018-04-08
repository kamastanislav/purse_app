package com.purse.purseclient;

import android.app.ProgressDialog;
import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.TextView;
import android.widget.Toast;

import com.purse.array_adapter.UserAdapter;
import com.purse.entity.UserData;
import com.purse.helper.Constants;
import com.purse.services.PurseService;
import com.purse.services.RestService;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;


public class UserActivity extends AppCompatActivity {
    private TextView fieldNick;
    private TextView fieldPassword;
    private TextView fieldCode;
    private ProgressDialog progress;
  //  private RestService restAdapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_user);

        Intent intent = getIntent();
        int code = intent.getIntExtra(Constants.USER_CODE, Constants.DEFAULT_CODE);
        Toast.makeText(UserActivity.this, "OK = " + code, Toast.LENGTH_LONG).show();

        fieldNick = (TextView)findViewById(R.id.nickName_user);
        fieldPassword = (TextView)findViewById(R.id.password_user);

        fieldCode = (TextView)findViewById(R.id.code_user);
        progress = new ProgressDialog(this);

     //   restAdapter = new RestService();

        initializationData(code);

    }

    private void initializationData(int code) {
        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();

        Call<UserData> call = RestService.getService().getUser(code);

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

                        Call<Boolean> res = RestService.getService().logoutUser(userData.Code);
                        res.enqueue(new Callback<Boolean>() {
                            @Override
                            public void onResponse(Call<Boolean> call, Response<Boolean> response) {
                                Toast.makeText(UserActivity.this, "OK " + response.body(), Toast.LENGTH_LONG).show();
                            }

                            @Override
                            public void onFailure(Call<Boolean> call, Throwable t) {

                            }
                        });

                    } else {
                        Toast.makeText(UserActivity.this, "NO", Toast.LENGTH_LONG).show();
                    }
                } else {
                    Toast.makeText(UserActivity.this, "NO", Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void onFailure(Call<UserData> call, Throwable t) {

            }
        });

      /*  apiService.getUser(code, new Callback<UserData>(){

            @Override
            public void success(UserData userData, Response response) {
                fieldCode.setText(String.valueOf(userData.Code));
                fieldNick.setText(userData.NickName);
                fieldPassword.setText(userData.Password);
                progress.dismiss();
               /* restService.getService().logoutUser(userData.Code, new Callback<Boolean>() {
                    @Override
                    public void success(Boolean aBoolean, Response response) {
                        Toast.makeText(UserActivity.this, "OK " + aBoolean, Toast.LENGTH_LONG).show();
                    }

                    @Override
                    public void failure(RetrofitError error) {
                        Toast.makeText(UserActivity.this,error.toString(), Toast.LENGTH_LONG).show();
                    }
                });*/
        /*    }

            @Override
            public void failure(RetrofitError error) {
                progress.dismiss();
                Toast.makeText(UserActivity.this, error.toString(), Toast.LENGTH_LONG).show();
            }
        });*/



    }
}
