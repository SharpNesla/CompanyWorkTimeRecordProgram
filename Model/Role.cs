using System.ComponentModel;

namespace Employees.Model
{
    /// <summary>
    /// Перечисление Role используется для хранения данных
    /// о правах пользователей в системе. 
    /// </summary>
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