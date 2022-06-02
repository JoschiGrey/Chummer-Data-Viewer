﻿using System.Diagnostics;
using Blazorise;
using Blazorise.DataGrid;
using Blazorise.Extensions;
using Chummer_Database.Classes;
using Chummer_Database.Classes.DataStructures;
using Chummer_Database.Enums;
using EnumExtensions = Chummer_Database.Extensions.EnumExtensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using static Chummer_Database.Classes.XmlLoader;

namespace Chummer_Database.Pages;

public partial class WeaponTable : ComponentBase
{
    private HashSet<Weapon>? Weapons { get; set; }
    private Weapon? SelectedWeapon { get; set; }
    private HashSet<Category>? WeaponCategories { get; set; }
    private HashSet<Book>? Books { get; set; }

    private DataGrid<Weapon>? _dataGridRef;
    private int _pageSize = 10;




    
    protected override async Task OnInitializedAsync()
    {
        if (!CreatedXml.Contains(typeof(WeaponsXmlRoot)))
        {
            await LoadAllAsync(Client, Logger);
        }
        Weapons = WeaponXmlData?.Weapons.ToHashSet();
        WeaponCategories = WeaponXmlData?.WeaponCategories.ToHashSet();
        Books = BooksXmlData?.Books;
    }

    private Task OnSelectedLegalitiesChanged(IReadOnlyList<Legality> list)
    {
        _selectedLegalities = list.ToHashSet();
        
        return Task.CompletedTask;
    }


    private readonly IEnumerable<Legality> _legalities = EnumExtensions.GetValues<Legality>();

    private HashSet<string>? _selectedCategories;
    private string _nameFilter = string.Empty;
    private NumericRange _costRange = new();
    private NumericRange _availRange = new();
    private HashSet<Legality>? _selectedLegalities;
    private NumericRange _damageRange = new();
    private NumericRange _concealRange = new();
    private NumericRange _reachRange = new();
    private NumericRange _ammoRange = new();
    private HashSet<string> _selectedAmmoCategories = new();
    private string _accessoryFilter = string.Empty;
    private string _skillFilter = string.Empty;
    private HashSet<string>? _selectedBooks;


    private bool OnCustomFilter(Weapon weapon)
    {
        var check = CheckName() &&
                    CheckPrice() &&
                    CheckLegality() &&
                    CheckCategory() &&
                    CheckAvailability() &&
                    CheckBooks() &&
                    CheckDamage() &&
                    CheckConceal() &&
                    CheckReach() &&
                    CheckAmmoCapacity() &&
                    CheckAmmoCategory() &&
                    CheckAccessories() &&
                    CheckSkill();
        //TODO: Rework this to use individual callbacks on the inputs that update a list value to not recheck everything
        return check;
        //Name
        bool CheckName()
        {
            return weapon.Name.Contains(_nameFilter);
        }
        
        //Price
        bool CheckPrice()
        {
            return _costRange.NumberFilter(weapon.Cost);
        }
        
        //Legality
        bool CheckLegality()
        {
            return weapon.Availability != null && _selectedLegalities.ListFilter(weapon.Availability.Legality);
        }

        //Availability
        bool CheckAvailability()
        {
            return _availRange.NumberFilter(weapon.Availability?.AvailabilityInt);
        }
        
        //TODO: Source Book this requires the deserialization of books.xml
        bool CheckBooks()
        {
            return _selectedBooks.ListFilter(weapon.Book?.Name);
        }
        
        //Category
        bool CheckCategory()
        {
            if (_selectedCategories is null)
                return true;
            if (_selectedCategories.Contains("All"))
                return true;

            return _selectedCategories.ListFilter(weapon.Category?.Name);
        }
        
        //Ap TODO: ApInt erstellen in Weapon
        
        //DMG
        bool CheckDamage()
        {
            return _damageRange.NumberFilter(weapon.Damage?.DamageAmount);
        }

        //Acc TODO: Accuracy Int erstellen
        
        
        //Conceal
        bool CheckConceal()
        {
            return _concealRange.NumberFilter(weapon.Conceal);
        }
        
        //Reach
        bool CheckReach()
        {
            return _reachRange.NumberFilter(weapon.Reach);
        }
        
        //FireModes TODO:(Muss noch in Weapon aufbereitet werden)
        //Recoil Comp TODO:Int muss noch bereitgestellt werden
        
        //Mag Size
        bool CheckAmmoCapacity()
        {
            return _ammoRange.NumberFilter(weapon.Ammo?.MagazineSize);
        }
        
        //Ammo Cat
        bool CheckAmmoCategory()
        {
            if (_selectedAmmoCategories.Contains("All"))
                return true;
            
            return _selectedAmmoCategories.ListFilter(weapon.Ammo?.AmmoCategory);
        }
        
        //Accessories
        bool CheckAccessories()
        {
            if (weapon.Accessories.IsNullOrEmpty())
                return true;

            if (_accessoryFilter.IsNullOrEmpty())
                return true;
            
            foreach (var weaponAccessory in weapon.Accessories)
            {
                if (weaponAccessory.Name.Contains(_accessoryFilter))
                    return true;
            }
            return false;
        }
        
        //Skill
        bool CheckSkill()
        {
            if (weapon.Skill is null)
                throw new ArgumentNullException(nameof(weapon.Skill));

            return weapon.Skill.Name.Contains(_skillFilter);
        }
    }
}
