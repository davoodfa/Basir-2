using Basir.Application.Globalization.DTOs;
using Basir.Application.Globalization.ViewModels;
using Basir.Domain.Entities.Globalization;
using Basir.Domain.Enums;

namespace Basir.Application.Globalization.Mapping;

public static class GlobalizationMapping
{
    public static LanguageDto ToDto(this Language entity)
    {
        return new LanguageDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            NativeName = entity.NativeName,
            Direction = entity.Direction == Direction.Rtl ? "rtl" : "ltr",
            FlagIcon = entity.FlagIcon,
            IsDefault = entity.IsDefault
        };
    }

    public static ThemeDto ToDto(this Theme entity)
    {
        return new ThemeDto
        {
            Id = entity.Id,
            Name = entity.Name,
            DisplayName = entity.DisplayName,
            ThemeType = entity.ThemeType.ToString(),
            PrimaryColor = entity.PrimaryColor,
            SecondaryColor = entity.SecondaryColor,
            IsDefault = entity.IsDefault,
            CssVariables = entity.CssVariables
        };
    }

    public static UserPreferenceDto ToDto(this UserPreference entity)
    {
        return new UserPreferenceDto
        {
            LanguageId = entity.LanguageId,
            ThemeId = entity.ThemeId,
            TimeZoneId = entity.TimeZoneId,
            CalendarType = entity.CalendarType,
            NumberFormat = entity.NumberFormat,
            FirstDayOfWeek = entity.FirstDayOfWeek,
            ItemsPerPage = entity.ItemsPerPage
        };
    }

    public static UserPreference ToEntity(this UserPreferenceDto dto, Guid userId)
    {
        return new UserPreference
        {
            UserId = userId,
            LanguageId = dto.LanguageId,
            ThemeId = dto.ThemeId,
            TimeZoneId = dto.TimeZoneId,
            CalendarType = dto.CalendarType,
            NumberFormat = dto.NumberFormat,
            FirstDayOfWeek = dto.FirstDayOfWeek,
            ItemsPerPage = dto.ItemsPerPage
        };
    }

    public static UserPreference ApplyDto(this UserPreference entity, UserPreferenceDto dto)
    {
        entity.LanguageId = dto.LanguageId;
        entity.ThemeId = dto.ThemeId;
        entity.TimeZoneId = dto.TimeZoneId;
        entity.CalendarType = dto.CalendarType;
        entity.NumberFormat = dto.NumberFormat;
        entity.FirstDayOfWeek = dto.FirstDayOfWeek;
        entity.ItemsPerPage = dto.ItemsPerPage;
        return entity;
    }

    public static PreferencesViewModel ToViewModel(this UserPreferenceDto dto)
    {
        return new PreferencesViewModel
        {
            LanguageId = dto.LanguageId,
            ThemeId = dto.ThemeId,
            TimeZoneId = dto.TimeZoneId,
            CalendarType = dto.CalendarType,
            NumberFormat = dto.NumberFormat,
            FirstDayOfWeek = dto.FirstDayOfWeek,
            ItemsPerPage = dto.ItemsPerPage
        };
    }

    public static UserPreferenceDto ToDto(this PreferencesViewModel vm)
    {
        return new UserPreferenceDto
        {
            LanguageId = vm.LanguageId,
            ThemeId = vm.ThemeId,
            TimeZoneId = vm.TimeZoneId,
            CalendarType = vm.CalendarType,
            NumberFormat = vm.NumberFormat,
            FirstDayOfWeek = vm.FirstDayOfWeek,
            ItemsPerPage = vm.ItemsPerPage
        };
    }
}
