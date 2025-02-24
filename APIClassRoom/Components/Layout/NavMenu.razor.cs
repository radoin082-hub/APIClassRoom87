using APIClassRoom.Components.Pages;
using Microsoft.AspNetCore.Components;

namespace APIClassRoom.Components.Layout;

public partial class NavMenu
{
    [Inject] public HttpClient Client { get; set; }
    private bool isDropdownOpen = false;
    private AIBot? aiBotComponent;
    private void ToggleDropdown()
    {
        isDropdownOpen = !isDropdownOpen;
    }
}