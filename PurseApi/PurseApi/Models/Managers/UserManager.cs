using PurseApi.Models.Entities;
using PurseApi.Models.Helper;
using PurseApi.Models.Helpers;
using PurseApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PurseApi.Models.Managers
{
    public class UserManager
    {
        public static List<UserData> GetUsers(int? famalyCode)
        {
            UserRepository userRepo;
            if (famalyCode.HasValue)
            {
                userRepo = new UserRepository((int)Constants.UserAction.Family);
                return userRepo.GetList(famalyCode.Value);
            }
            else
            {
                userRepo = new UserRepository((int)Constants.UserAction.List);
                return userRepo.GetList();
            }
        }

        public static UserData GetUser(int userCode)
        {
            UserRepository userRepo = new UserRepository((int)Constants.UserAction.Code);
            var user = userRepo.GetUser(userCode);
            return user;
        }

        public static bool RestorePassword(UserLogin login)
        {
            UserRepository userRepo = new UserRepository((int)Constants.UserAction.Restore);
            var user = userRepo.GetList(login.NickName).FirstOrDefault();
            if (user != null)
            {
                var password = SendNewPassword(string.Format("{0} {1} ({2})", user.FirstName, user.LastName, user.NickName), user.Email, login.Password, user.Code);
                user.Password = password;
                userRepo.SetActionCode((int)Constants.UserAction.Code);
                user = userRepo.UpdateUserData(user.Code, user, new List<int>() { (int)Constants.UserField.Password });
                if (user != null)
                    return true;
            }
            throw new NotImplementedException();
        }

        private static string SendNewPassword(string name, string email, string password, int code)
        {
            int port = 587;
            string smtp = "smtp.mail.ru";
            bool SSL = true;
            NetworkCredential login = new NetworkCredential("e.gallery.sys@mail.ru", "egallery2905Sys");
            SmtpClient client = new SmtpClient(smtp);
            MailMessage msg;
            Random random = new Random();

            int verificationCode = random.Next(1000000, 9999999);
            password += verificationCode;
            client.Port = port;
            client.EnableSsl = SSL;
            client.Credentials = login;
            msg = new MailMessage()
            {
                From = new MailAddress("e.gallery.sys@mail.ru", "E-Family Purse", Encoding.UTF8)
            };
            msg.To.Add(new MailAddress(email));
            msg.Subject = "Восстановление пароля.";
            msg.Body = "Добрый день, " + name + ", Ваш новый пароль: " + "<b>" + password + "</b>";
            msg.BodyEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.Priority = MailPriority.Normal;
            msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            client.Send(msg);

            return password;
        }

        public static int CreateNewUser(UserData user)
        {
            try
            {
                var userSession = UserSession.Create(userRegistration: user);
                return userSession != null ? userSession.User.Code : Constants.DEFAULT_CODE;
            }
            catch (Exception)
            {
                return Constants.DEFAULT_CODE;
            }
        }

        public static UserData UpdateUser(UserData user, List<int> fields)
        {
            UserRepository userRepo = new UserRepository((int)Constants.UserAction.Code);

            user = userRepo.UpdateUserData(user.Code, user, fields);
            return user;
        }

        public static bool LogoutUser()
        {
            try
            {
                var task = new Task(() => HistoryManager.DeleteInformation());
                task.Start();
                return UserSession.DestroyIfExpired();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static int LoginUser(UserLogin userLogin)
        {
            var userSession = UserSession.Create(userLogin.NickName, userLogin.Password);
            return userSession != null ? userSession.User.Code : Constants.DEFAULT_CODE;
        }

        public static UserData UpdateUserData(UserData user)
        {
            var userData = UserSession.Current.User;
            
            if (userData != null)
            {
                var userRepo = new UserRepository((int)Constants.UserAction.Code);
                var fields = CheckFieldsForUpdate(user, userData);
               
                if (fields.Any())
                {
                    user.LastLogin = Constants.TotalMilliseconds;
                    userRepo.UpdateUserData(userData.Code, user, fields);
                    var userSession = UserSession.UpdateSession();
                    return userSession != null ? userSession.User : null;
                }
            }
            throw new Exception();
        }

        private static List<int> CheckFieldsForUpdate(UserData user, UserData userData)
        {
            List<int> fields = new List<int>();

            if (user.FirstName != userData.FirstName)
                fields.Add((int)Constants.UserField.FirstName);

            if (user.LastName != userData.LastName)
                fields.Add((int)Constants.UserField.LastName);

            if (user.NickName != userData.NickName)
                fields.Add((int)Constants.UserField.NickName);

            if (user.Email != userData.Email)
                fields.Add((int)Constants.UserField.Email);

            if (user.Phone != userData.Phone)
                fields.Add((int)Constants.UserField.Phone);

            if (user.Cash != userData.Cash)
                fields.Add((int)Constants.UserField.Cash);

            if (user.Birthday != userData.Birthday)
                fields.Add((int)Constants.UserField.Birthday);

            if (fields.Any())
                fields.Add((int)Constants.UserField.LastLogin);
            return fields;
        }

        public static bool UpdateUserData(string password)
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                var userRepo = new UserRepository((int)Constants.UserAction.Code);
                user.Password = password;
                user.LastLogin = Constants.TotalMilliseconds;
                userRepo.UpdateUserData(user.Code, user, new List<int>() { (int)Constants.UserField.Password, (int)Constants.UserField.LastLogin });
                return true;
            }
            throw new Exception();
        }

        public static bool CheckPassword(string password)
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                var userRepo = new UserRepository();
                return userRepo.CheckUserPassword(user.Code, password);
            }
            throw new Exception();
        }

        public static bool IsUnique(int field, string value)
        {
            UserRepository userRepo = new UserRepository();
            return !userRepo.IsUnique((Constants.UserField)field, value);
        }

        public static bool DeleteUser(int code)
        {
            UserRepository userRepo = new UserRepository((int)Constants.UserAction.Code);
            return userRepo.DeleteUser(code);
        }
    }
}