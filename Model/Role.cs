using System.ComponentModel;

namespace Employees.Model
{
    public enum Role
    {
        [Description("Чтение и запись")]
        Manager,
        [Description("Только чтение")]
        ManagerRO,
        [Description("Не является пользователем")]
        Undefined
    }
}