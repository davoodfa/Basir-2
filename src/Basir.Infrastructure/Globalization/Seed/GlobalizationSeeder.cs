using Basir.Domain.Entities.Globalization;
using Basir.Domain.Enums;
using Basir.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Basir.Infrastructure.Globalization.Seed;

public class GlobalizationSeeder
{
    private readonly AppIdentityDbContext _context;

    public GlobalizationSeeder(AppIdentityDbContext context) => _context = context;

    public async Task SeedAsync()
    {
        if (await _context.Set<Language>().AnyAsync())
            return;

        var persian = new Language
        {
            Code = "fa-IR",
            Name = "Persian",
            NativeName = "فارسی",
            Direction = Direction.Rtl,
            FlagIcon = "bi-flag",
            IsDefault = true,
            IsEnabled = true,
            SortOrder = 1,
            CreatedAt = DateTime.UtcNow
        };

        var english = new Language
        {
            Code = "en-US",
            Name = "English",
            NativeName = "English",
            Direction = Direction.Ltr,
            FlagIcon = "bi-flag",
            IsDefault = false,
            IsEnabled = true,
            SortOrder = 2,
            CreatedAt = DateTime.UtcNow
        };

        var arabic = new Language
        {
            Code = "ar-SA",
            Name = "Arabic",
            NativeName = "العربية",
            Direction = Direction.Rtl,
            FlagIcon = "bi-flag",
            IsDefault = false,
            IsEnabled = true,
            SortOrder = 3,
            CreatedAt = DateTime.UtcNow
        };

        _context.Set<Language>().AddRange(persian, english, arabic);
        await _context.SaveChangesAsync();

        var lightTheme = new Theme
        {
            Name = "light",
            DisplayName = "روشن",
            ThemeType = ThemeType.Light,
            PrimaryColor = "#0d6efd",
            SecondaryColor = "#6c757d",
            IsDefault = true,
            IsEnabled = true,
            SortOrder = 1,
            CreatedAt = DateTime.UtcNow
        };

        var darkTheme = new Theme
        {
            Name = "dark",
            DisplayName = "تاریک",
            ThemeType = ThemeType.Dark,
            PrimaryColor = "#212529",
            SecondaryColor = "#495057",
            IsDefault = false,
            IsEnabled = true,
            SortOrder = 2,
            CreatedAt = DateTime.UtcNow
        };

        var blueTheme = new Theme
        {
            Name = "blue",
            DisplayName = "آبی",
            ThemeType = ThemeType.Custom,
            PrimaryColor = "#0d6efd",
            SecondaryColor = "#0b5ed7",
            IsDefault = false,
            IsEnabled = true,
            CssVariables = "{\"bs-primary\": \"#0d6efd\", \"bs-primary-rgb\": \"13,110,253\"}",
            SortOrder = 3,
            CreatedAt = DateTime.UtcNow
        };

        _context.Set<Theme>().AddRange(lightTheme, darkTheme, blueTheme);
        await _context.SaveChangesAsync();

        var translations = new List<TranslationResource>
        {
            // Common module
            new() { LanguageId = persian.Id, Module = "Common", ResourceKey = "Shared.Save", Value = "ذخیره", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = english.Id, Module = "Common", ResourceKey = "Shared.Save", Value = "Save", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = arabic.Id, Module = "Common", ResourceKey = "Shared.Save", Value = "حفظ", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = persian.Id, Module = "Common", ResourceKey = "Shared.Cancel", Value = "لغو", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = english.Id, Module = "Common", ResourceKey = "Shared.Cancel", Value = "Cancel", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = arabic.Id, Module = "Common", ResourceKey = "Shared.Cancel", Value = "إلغاء", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = persian.Id, Module = "Common", ResourceKey = "Shared.Delete", Value = "حذف", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = english.Id, Module = "Common", ResourceKey = "Shared.Delete", Value = "Delete", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = persian.Id, Module = "Common", ResourceKey = "Shared.Edit", Value = "ویرایش", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = english.Id, Module = "Common", ResourceKey = "Shared.Edit", Value = "Edit", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = persian.Id, Module = "Common", ResourceKey = "Shared.Loading", Value = "در حال بارگذاری...", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = english.Id, Module = "Common", ResourceKey = "Shared.Loading", Value = "Loading...", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = persian.Id, Module = "Common", ResourceKey = "Shared.Confirm", Value = "تأیید", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = english.Id, Module = "Common", ResourceKey = "Shared.Confirm", Value = "Confirm", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = persian.Id, Module = "Common", ResourceKey = "Nav.Home", Value = "خانه", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = english.Id, Module = "Common", ResourceKey = "Nav.Home", Value = "Home", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = persian.Id, Module = "Common", ResourceKey = "Nav.Login", Value = "ورود", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = english.Id, Module = "Common", ResourceKey = "Nav.Login", Value = "Login", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = persian.Id, Module = "Common", ResourceKey = "Nav.Register", Value = "ثبت‌نام", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = english.Id, Module = "Common", ResourceKey = "Nav.Register", Value = "Register", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = persian.Id, Module = "Common", ResourceKey = "Nav.Logout", Value = "خروج", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = english.Id, Module = "Common", ResourceKey = "Nav.Logout", Value = "Logout", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = persian.Id, Module = "Common", ResourceKey = "Nav.ManageAccount", Value = "مدیریت حساب", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = english.Id, Module = "Common", ResourceKey = "Nav.ManageAccount", Value = "Manage Account", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = persian.Id, Module = "Common", ResourceKey = "Footer.Copyright", Value = "تمام حقوق محفوظ است.", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = english.Id, Module = "Common", ResourceKey = "Footer.Copyright", Value = "All rights reserved.", CreatedAt = DateTime.UtcNow },

            // Globalization module
            new() { LanguageId = persian.Id, Module = "Globalization", ResourceKey = "LanguageSwitcher.Label", Value = "زبان", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = english.Id, Module = "Globalization", ResourceKey = "LanguageSwitcher.Label", Value = "Language", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = persian.Id, Module = "Globalization", ResourceKey = "ThemeSwitcher.Label", Value = "قالب", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = english.Id, Module = "Globalization", ResourceKey = "ThemeSwitcher.Label", Value = "Theme", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = persian.Id, Module = "Globalization", ResourceKey = "Preferences.Title", Value = "تنظیمات", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = english.Id, Module = "Globalization", ResourceKey = "Preferences.Title", Value = "Preferences", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = persian.Id, Module = "Globalization", ResourceKey = "Preferences.Saved", Value = "تنظیمات با موفقیت ذخیره شد.", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = english.Id, Module = "Globalization", ResourceKey = "Preferences.Saved", Value = "Preferences saved successfully.", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = persian.Id, Module = "Globalization", ResourceKey = "Preferences.Language", Value = "زبان", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = english.Id, Module = "Globalization", ResourceKey = "Preferences.Language", Value = "Language", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = persian.Id, Module = "Globalization", ResourceKey = "Preferences.Theme", Value = "قالب نمایش", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = english.Id, Module = "Globalization", ResourceKey = "Preferences.Theme", Value = "Theme", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = persian.Id, Module = "Globalization", ResourceKey = "Preferences.TimeZone", Value = "منطقه زمانی", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = english.Id, Module = "Globalization", ResourceKey = "Preferences.TimeZone", Value = "Time Zone", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = persian.Id, Module = "Globalization", ResourceKey = "Preferences.Calendar", Value = "نوع تقویم", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = english.Id, Module = "Globalization", ResourceKey = "Preferences.Calendar", Value = "Calendar Type", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = persian.Id, Module = "Globalization", ResourceKey = "Preferences.NumberFormat", Value = "فرمت اعداد", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = english.Id, Module = "Globalization", ResourceKey = "Preferences.NumberFormat", Value = "Number Format", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = persian.Id, Module = "Globalization", ResourceKey = "Preferences.ItemsPerPage", Value = "تعداد آیتم در صفحه", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = english.Id, Module = "Globalization", ResourceKey = "Preferences.ItemsPerPage", Value = "Items Per Page", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = persian.Id, Module = "Globalization", ResourceKey = "Preferences.FirstDayOfWeek", Value = "اولین روز هفته", CreatedAt = DateTime.UtcNow },
            new() { LanguageId = english.Id, Module = "Globalization", ResourceKey = "Preferences.FirstDayOfWeek", Value = "First Day of Week", CreatedAt = DateTime.UtcNow },
        };

        _context.Set<TranslationResource>().AddRange(translations);
        await _context.SaveChangesAsync();
    }
}
