using WpfApp1.Models;

namespace WpfApp1
{
    public static class AppState
    {
        // Текущий авторизованный пользователь
        public static Users CurrentUser { get; set; }

        // Текущая роль пользователя
        public static string CurrentUserRole => CurrentUser?.Role?.RoleName ?? "Гость";

        // Проверка, авторизован ли пользователь
        public static bool IsAuthenticated => CurrentUser != null;

        // Проверка роли пользователя
        public static bool IsAdmin => CurrentUser?.RoleID == 1;
        public static bool IsEmployee => CurrentUser?.RoleID == 2;
        public static bool IsClient => CurrentUser?.RoleID == 3;

        // Метод для очистки состояния
        public static void Clear()
        {
            CurrentUser = null;
        }

        // Метод для установки пользователя после авторизации
        public static void SetCurrentUser(Users user)
        {
            CurrentUser = user;
        }

        // Получение полного имени пользователя
        public static string CurrentUserName
        {
            get
            {
                if (CurrentUser == null) return "Гость";

                if (!string.IsNullOrEmpty(CurrentUser.LastName) && !string.IsNullOrEmpty(CurrentUser.Name))
                {
                    return $"{CurrentUser.LastName} {CurrentUser.Name}";
                }
                else if (!string.IsNullOrEmpty(CurrentUser.Name))
                {
                    return CurrentUser.Name;
                }
                else if (!string.IsNullOrEmpty(CurrentUser.Login))
                {
                    return CurrentUser.Login;
                }

                return "Пользователь";
            }
        }
    }
}