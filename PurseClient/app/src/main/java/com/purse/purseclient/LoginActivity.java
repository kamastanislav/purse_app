package com.purse.purseclient;

import android.content.Intent;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;

import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

import retrofit.Callback;
import retrofit.RetrofitError;
import retrofit.client.Response;

import com.purse.entity.UserData;
import com.purse.services.RestService;

import java.util.List;


public class LoginActivity extends AppCompatActivity {
    EditText fieldNick;
    EditText fieldPassword;
    RestService restService;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);
        fieldNick = (EditText) findViewById(R.id.nickName);
        fieldPassword = (EditText) findViewById(R.id.password);

        restService = new RestService();
    }

    public  void  login(View view) {
        String login = fieldNick.getText().toString();
        String password = fieldPassword.getText().toString();
        if (!login.isEmpty() && !password.isEmpty())  {
            restService.getService().registration(new Callback<List<UserData>>(){
                @Override
                public void success(List<UserData> user, Response response) {

                    Toast.makeText(LoginActivity.this, "Student Record Deleted", Toast.LENGTH_LONG).show();
                }

                @Override
                public void failure(RetrofitError error) {
                    Toast.makeText(LoginActivity.this, error.getMessage().toString(), Toast.LENGTH_LONG).show();

                }
            });
        }
    }

    public void registrationView(View view) {
        Intent intent = new Intent(this, RegistrationActivity.class);

        startActivity(intent);
    }
}

