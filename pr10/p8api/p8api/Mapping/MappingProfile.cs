using AutoMapper;
using p8api.DTOs;
using p8api.Models;

namespace p8api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<decimal?, decimal>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<bool?, bool>().ConvertUsing((src, dest) => src ?? dest);

            CreateMap<Role, RoleDTO>();
            CreateMap<CreateRoleDTO, Role>(MemberList.Source);
            CreateMap<UpdateRoleDTO, Role>(MemberList.Source);
            CreatePatchMap<PatchRoleDTO, Role>();

            CreateMap<User, UserDTO>();
            CreateMap<User, UserShortDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                    src.MiddleName == null
                        ? src.Surname + " " + src.NameUser
                        : src.Surname + " " + src.NameUser + " " + src.MiddleName));
            CreateMap<CreateUserDTO, User>(MemberList.Source);
            CreateMap<UpdateUserDTO, User>(MemberList.Source);
            CreatePatchMap<PatchUserDTO, User>();

            CreateMap<Category, CategoryDTO>();
            CreateMap<Category, CategoryShortDTO>();
            CreateMap<CreateCategoryDTO, Category>(MemberList.Source);
            CreateMap<UpdateCategoryDTO, Category>(MemberList.Source);
            CreatePatchMap<PatchCategoryDTO, Category>();

            CreateMap<Product, ProductDTO>();
            CreateMap<Product, ProductShortDTO>();
            CreateMap<CreateProductDTO, Product>(MemberList.Source);
            CreateMap<UpdateProductDTO, Product>(MemberList.Source);
            CreatePatchMap<PatchProductDTO, Product>();

            CreateMap<Cart, CartDTO>();
            CreateMap<CreateCartDTO, Cart>(MemberList.Source);
            CreateMap<UpdateCartDTO, Cart>(MemberList.Source);
            CreatePatchMap<PatchCartDTO, Cart>();

            CreateMap<CartItem, CartItemDTO>();
            CreateMap<CreateCartItemDTO, CartItem>(MemberList.Source);
            CreateMap<UpdateCartItemDTO, CartItem>(MemberList.Source);
            CreatePatchMap<PatchCartItemDTO, CartItem>();

            CreateMap<OrderStatus, OrderStatusDTO>();
            CreateMap<CreateOrderStatusDTO, OrderStatus>(MemberList.Source);
            CreateMap<UpdateOrderStatusDTO, OrderStatus>(MemberList.Source);
            CreatePatchMap<PatchOrderStatusDTO, OrderStatus>();

            CreateMap<Order, OrderDTO>();
            CreateMap<CreateOrderDTO, Order>(MemberList.Source);
            CreateMap<UpdateOrderDTO, Order>(MemberList.Source);
            CreatePatchMap<PatchOrderDTO, Order>();

            CreateMap<OrderItem, OrderItemDTO>();
            CreateMap<CreateOrderItemDTO, OrderItem>(MemberList.Source);
            CreateMap<UpdateOrderItemDTO, OrderItem>(MemberList.Source);
            CreatePatchMap<PatchOrderItemDTO, OrderItem>();

            CreateMap<ProductReservation, ReservationDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.ReleasedDate == null));
            CreateMap<CreateReservationDTO, ProductReservation>(MemberList.Source);
        }

        private void CreatePatchMap<TSource, TDestination>()
        {
            CreateMap<TSource, TDestination>(MemberList.Source)
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
