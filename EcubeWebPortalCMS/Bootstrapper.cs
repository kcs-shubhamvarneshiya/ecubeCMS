// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="Bootstrapper.cs" company="string.Empty">
//     Copyright ©  2016
// </copyright>
// <summary>KCSPL</summary>
// ***********************************************************************

/// <summary>
/// The EcubeWebPortalCMS namespace.
/// </summary>
namespace EcubeWebPortalCMS
{
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Models;
    using Microsoft.Practices.Unity;
    using Unity.Mvc4;

    /// <summary>
    /// Class Boots trapper.
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns>IUnity Container.</returns>
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            return container;
        }

        /// <summary>
        /// Registers the types.
        /// </summary>
        /// <param name="container">The container.</param>
        public static void RegisterTypes(IUnityContainer container)
        {
        }

         /// <summary>
        /// Builds the unity container.
        /// </summary>
        /// <returns>IUnity Container.</returns>
        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<IActivityLogCommand, ActivityLogCommand>();
            container.RegisterType<IEmailLogCommand, EmailLogCommand>();
            container.RegisterType<IErrorLogCommand, ErrorLogCommand>();
            container.RegisterType<IIPAddressCommand, IPAddressCommand>();
            container.RegisterType<ISMSLogCommand, SMSLogCommand>();
            container.RegisterType<IRoleCommand, RoleCommand>();
            container.RegisterType<IUserCommand, UserCommand>();
            container.RegisterType<IUserLogCommand, UserLogCommand>();
            container.RegisterType<IBanquetCommand, BanquetCommand>();
            container.RegisterType<IRoomBookingCommand, RoomBookingCommand>();
            container.RegisterType<ICMSConfigCommand, CMSConfigCommand>();

            container.RegisterType<IBanquetBookingRequestCommand, BanquetBookingRequestCommand>();
            container.RegisterType<IRoomBookingRequestCommand, RoomBookingRequestCommand>();
            container.RegisterType<IMovieCommand, MovieCommand>();
            container.RegisterType<IMovieShowCommand, MovieShowCommand>();
            container.RegisterType<IMovieTheatreCommand, MovieTheatreCommand>();
            container.RegisterType<IServiceSupportTypeCommand, ServiceSupportTypeCommand>();
            container.RegisterType<IServiceSupportLogCommand, ServiceSupportLogCommand>();
            container.RegisterType<IServiceSupportCommand, ServiceSupportCommand>();
            container.RegisterType<ICustomPageCommand, CustomPageCommand>();
            container.RegisterType<IDocumentLibraryCommand, DocumentLibraryCommand>();
            container.RegisterType<IEventCategoryCommand, EventCategoryCommand>();
            container.RegisterType<IEventTicketCategoryCommand, EventTicketCategoryCommand>();
            container.RegisterType<IEventBookigCommand, EventBookigCommand>();
            container.RegisterType<IMovieTicketCommand, MovieTicketCommand>();
            container.RegisterType<IEventTicketCommand, EventTicketCommand>();
            container.RegisterType<IEventBookingForMemberCommand, EventBookingForMemberCommand>();
            container.RegisterType<IInquiryCategoryCommand, InquiryCategoryCommand>();
            RegisterTypes(container);
            return container;
        }
    }
}
