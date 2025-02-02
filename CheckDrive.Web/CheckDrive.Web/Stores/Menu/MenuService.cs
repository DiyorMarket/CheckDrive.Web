namespace CheckDrive.Web.Stores.Menu;

public class MenuService : IMenuService
{
    public List<Object> GetMenuItems(string role)
    {
        List<Object> menuItems = new List<Object>();
        menuItems.Add(new
        {
            text = "Asosiy",
            iconCss = "fa-solid fa-home",
            url = "/",
        });

        menuItems.Add(new
        {
            text = "Qarzlar",
            iconCss = "fa-solid fa-file-invoice-dollar",
            url = "/debts",
        });

        menuItems.Add(new
        {
            text = "Umumiy",
            separator = true,
        });

        menuItems.Add(new
        {
            text = "Ishchilar",
            iconCss = "fas fa-users",
            url = "/employees"
        });

        menuItems.Add(new
        {
            text = "Avtomobillar",
            iconCss = "fas fa-car",
            url = "/cars"
        });

        menuItems.Add(new
        {
            text = "Yoqilg'ilar",
            iconCss = "fa-solid fa-gas-pump",
            url = "/oilMarks"
        });

        menuItems.Add(new
        {
            text = "Tekshiruvlar",
            separator = true,
        });

        menuItems.Add(new
        {
            text = "Shifokor",
            iconCss = "fa-solid fa-briefcase-medical",
            url = "/doctorreviews"
        });

        menuItems.Add(new
        {
            text = "Mexanik (Topshirish)",
            iconCss = "fa-solid fa-tools",
            url = "/mechanichandovers"
        });

        menuItems.Add(new
        {
            text = "Operator",
            iconCss = "fa-solid fa-headset",
            url = "/OperatorReviews"
        });

        menuItems.Add(new
        {
            text = "Mexanik (Qabul qilish)",
            iconCss = "fa-solid fa-car-on",
            url = "/mechanicacceptances"
        });

        menuItems.Add(new
        {
            text = "Dispetcher",
            iconCss = "fa-solid fa-user-gear",
            url = "/DispatcherReviews"
        });

        if (role == "1")
        {
        }
        else if (role == "3")
        {
            menuItems.Add(new
            {
                text = "Asosiy sahifa",
                iconCss = "fa-solid fa-house",
                url = "/doctorreviews/personalindex"
            });

            menuItems.Add(new
            {
                text = "Tarixni ko'rish",
                iconCss = "fa-solid fa-clock-rotate-left",
                url = "/doctorreviews/historyindexforpersonalpage"
            });
        }
        else if (role == "4")
        {
            menuItems.Add(new
            {
                text = "Asosiy sahifa",
                iconCss = "fa-solid fa-house",
                url = "/operatorreviews/personalindex"
            });

            menuItems.Add(new
            {
                text = " Hisobot",
                iconCss = "fa-regular fa-clipboard",
                url = "/operatorreviews/reportindexforpersonalpage"
            });

            menuItems.Add(new
            {
                text = "Tarixni ko'rish",
                iconCss = "fa-solid fa-clock-rotate-left",
                url = "/operatorreviews/historyindexforpersonalpage"
            });
        }
        else if (role == "5")
        {
            menuItems.Add(new
            {
                text = "Asosiy sahifa",
                iconCss = "fa-solid fa-house",
                url = "/dispatcherreviews/personalindex"
            });

            menuItems.Add(new
            {
                text = "Tarixni ko'rish",
                iconCss = "fa-solid fa-clock-rotate-left",
                url = "/dispatcherreviews/historyindexforpersonalpage"
            });
        }
        else if (role == "6")
        {

            menuItems.Add(new
            {
                id = "handoversItem",
                text = "Topshirish",
                iconCss = "fa-solid fa-tools",
                url = "/mechanichandovers/personalindex"
            });


            menuItems.Add(new
            {
                id = "acceptancesItem",
                text = "Qabul qilish",
                iconCss = "fa-solid fa-car-on",
                url = "/mechanicacceptances/personalindex"
            });

            menuItems.Add(new
            {
                id = "historyItem",
                text = "Tarixni ko'rish",
                iconCss = "fa-solid fa-clock-rotate-left",
                url = "/mechanicacceptances/historyindexforpersonalpage"
            });
        }

        return menuItems;
    }
}
