using Blazorise.DataGrid;
using Blazorise.Extensions;
using ChummerDataViewer.Classes;
using ChummerDataViewer.Classes.DataStructures;
using ChummerDataViewer.Classes.HelperMethods;
using ChummerDataViewer.Classes.XmlRoots;
using ChummerDataViewer.Enums;
using EnumExtensions = ChummerDataViewer.Extensions.EnumExtensions;
using Microsoft.AspNetCore.Components;
using static ChummerDataViewer.Classes.XmlLoader;

namespace ChummerDataViewer.Pages;

public partial class WeaponTable : ComponentBase
{
    private HashSet<Weapon>? Weapons { get; set; }
    private Weapon? SelectedWeapon { get; set; }
    private HashSet<Category>? WeaponCategories { get; set; }
    private HashSet<Book>? Books { get; set; }

    private DataGrid<Weapon>? _dataGridRef;
    private int _pageSize = 10;



    private Task OnSelectedRowChange(Weapon weapon)
    {
        SelectedWeapon = weapon;

        return Task.CompletedTask;
    }
    
    protected override async Task OnInitializedAsync()
    {
        if (!CreatedXml.Contains(typeof(WeaponsXmlRoot)))
        {
            await LoadAllAsync<WeaponsXmlRoot>(Client, Logger);
        }

        Weapons = Weapon.AllWeapons;
        WeaponCategories = Category.CategoryDictionary.Values.ToHashSet();
        Books = Book.BooksDictionary.Values.ToHashSet();
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
        var check =
            CheckSkill() && // THis too
            CheckName() &&
            CheckPrice() &&
            CheckLegality() &&
            CheckCategory() &&
            CheckAvailability() &&
            CheckBooks() &&
            CheckDamage() &&//Filters out "Fork Proboscis Throwing Syringe at default state
            CheckConceal() &&
            CheckReach() &&
            CheckAmmoCapacity() &&
            CheckAmmoCategory() &&
            CheckAccessories(); 
        
        //TODO: Rework this to use individual callbacks on the inputs that update a list value to not recheck everything
        return check;
        //Name
        bool CheckName()
        {
            return weapon.Name.Contains(_nameFilter, StringComparison.CurrentCultureIgnoreCase);
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
            return _selectedBooks.ListFilter(weapon.Source?.Book.Name);
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
            return _damageRange.NumberFilter(weapon.Damage?.DamageAmount, false);
        }

        //Acc TODO: Accuracy Int erstellen
        
        
        //Conceal
        bool CheckConceal()
        {
            return _concealRange.NumberFilter(weapon.Conceal, false);
        }
        
        //Reach
        bool CheckReach()
        {
            return _reachRange.NumberFilter(weapon.Reach, false);
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
            if (weapon.BaseAccessories.IsNullOrEmpty())
                return true;

            if (_accessoryFilter.IsNullOrEmpty())
                return true;
            
            foreach (var weaponAccessory in weapon.BaseAccessories)
            {
                if (weaponAccessory.Name.Contains(_accessoryFilter, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        
        //Skill
        bool CheckSkill()
        {
            if (weapon.Skill is null)
                throw new ArgumentNullException(nameof(weapon.Skill));

            return weapon.Skill.Name.Contains(_skillFilter, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
