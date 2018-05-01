package com.purse.purseclient;


import android.app.ProgressDialog;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;
import android.widget.Toast;

import com.purse.array_adapter.CashAdapter;
import com.purse.entity.HistoryCash;
import com.purse.helper.Constants;
import com.purse.services.RestService;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class LoggerFragment extends Fragment {
    private View view;
    private ProgressDialog progress;
    private ListView loggerListView;

    public LoggerFragment() {

    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        view = inflater.inflate(R.layout.fragment_logger, container, false);

        loggerListView = (ListView) view.findViewById(R.id.logger_list_view);
        progress = new ProgressDialog(view.getContext());
        loadData();

        return view;
    }

    private void loadData() {
        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();

        Call<List<HistoryCash>> call = RestService.getService().getHistoryUser(Constants.DEFAULT_CODE);
        call.enqueue(new Callback<List<HistoryCash>>() {
            @Override
            public void onResponse(Call<List<HistoryCash>> call, Response<List<HistoryCash>> response) {
                progress.dismiss();
                if (response.isSuccessful()) {
                    List<HistoryCash> historyCashes = response.body();

                    CashAdapter historyCashAdapter = new CashAdapter(view.getContext(), R.layout.view_cash_entity, historyCashes);

                    loggerListView.setAdapter(historyCashAdapter);

                } else
                    Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
            }

            @Override
            public void onFailure(Call<List<HistoryCash>> call, Throwable t) {

            }
        });
    }

}
