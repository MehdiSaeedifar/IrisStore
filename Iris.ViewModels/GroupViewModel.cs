using System.Collections.Generic;
using AutoMapper;
using AutoMapperContracts;

namespace Iris.ViewModels
{
    #region GroupViewModel
    public class GroupViewModel : IHaveCustomMappings
    {
        #region Properties
        public int Id { get; set; }           // Integer value of a checkbox
        public string Name { get; set; }      // String name of a checkbox
        public object Tags { get; set; }      // Object of html tags to be applied to checkbox, e.g.: 'new { tagName = "tagValue" }'
        public bool IsSelected { get; set; }  // Boolean value to select a checkbox on the list
        #endregion

        #region CreateMappings
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<CategoryViewModel, GroupViewModel>();
        }
        #endregion
    }
    #endregion

    #region GroupsViewModel
    public class GroupsViewModel
    {
        #region Properties
        public IList<GroupViewModel> AvailableGroups { get; set; }
        public IList<GroupViewModel> SelectedGroups { get; set; }
        public PostedGroups PostedGroups { get; set; }
        #endregion
    }
    #endregion

    #region PostedGroupsHelper
    // Helper class to make posting back selected values easier
    public class PostedGroups
    {
        public string[] GroupIDs { get; set; }
    }
    #endregion
}