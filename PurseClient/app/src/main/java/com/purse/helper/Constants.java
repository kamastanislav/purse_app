package com.purse.helper;

import com.purse.services.PurseService;
import com.purse.services.RestService;

public class Constants {
    public final static String USER_CODE = "USER_CODE";
    public final static int DEFAULT_CODE = 0;

    private static RestService restService = null;

    public static PurseService getRestService() {
        if (restService == null) {
            restService = new RestService();
        }

        return restService.getService();
    }

    public static String userName;
}
