package com.purse.purseclient;


import android.app.ProgressDialog;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.purse.entity.UserData;
import com.purse.services.RestService;

import java.util.Date;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class FamilyFragment extends Fragment {

    private View view;
    private ProgressDialog progress;
    private TextView field;
    private Button button;

    public FamilyFragment() {

    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_family, container, false);
        field = (TextView)view.findViewById(R.id.family_massage);
        progress = new ProgressDialog(view.getContext());
        button = (Button)view.findViewById(R.id.open_view_create_family);

        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();


        Call<Boolean> call = RestService.getService().havingFamily();

        call.enqueue(new Callback<Boolean>() {
            @Override
            public void onResponse(Call<Boolean> call, Response<Boolean> response) {
                progress.dismiss();
                if (response.isSuccessful()) {
                    Boolean havingFamily = response.body();

                    if (havingFamily != null) {
                        field.setText(String.valueOf(havingFamily));
                        if (havingFamily)
                            showFamilyInformation();
                        else
                            button.setVisibility(View.VISIBLE);

                    } else {
                        Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
                    }
                } else {
                    Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void onFailure(Call<Boolean> call, Throwable t) {

            }
        });
        return view;
    }

    private void showFamilyInformation() {
    }

}
