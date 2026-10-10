using AutoMapper;
using p8api.DTOs;
using p8api.Exceptions;
using p8api.Models;
using p8api.Repositories;

namespace p8api.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservations;
        private readonly IProductRepository _products;
        private readonly IUserRepository _users;
        private readonly IMapper _mapper;

        public ReservationService(IReservationRepository reservations, IProductRepository products, IUserRepository users, IMapper mapper)
        {
            _reservations = reservations;
            _products = products;
            _users = users;
            _mapper = mapper;
        }

        public async Task<PagedResult<ReservationDTO>> GetPageAsync(int page, int pageSize, CancellationToken ct)
        {
            (page, pageSize) = Paging.Normalize(page, pageSize);
            var items = await _reservations.GetPageAsync(page, pageSize, ct);
            var total = await _reservations.CountAsync(ct);
            return new PagedResult<ReservationDTO>(_mapper.Map<List<ReservationDTO>>(items), total);
        }

        public async Task<ReservationDTO?> GetByIdAsync(int id, CancellationToken ct)
        {
            var reservation = await _reservations.GetByIdAsync(id, ct);
            return reservation is null ? null : _mapper.Map<ReservationDTO>(reservation);
        }

        public async Task<ReservationDTO> ReserveAsync(CreateReservationDTO dto, CancellationToken ct)
        {
            await _users.EnsureExistsAsync(dto.UserId, $"Пользователь с Id {dto.UserId} не найден.", ct);

            var product = await _products.FindAsync(dto.ProductId, ct)
                ?? throw new RelatedEntityNotFoundException($"Товар с Id {dto.ProductId} не найден.");

            if (!product.IsAvailable)
            {
                throw new BusinessRuleException($"Товар «{product.ProductName}» сейчас недоступен для бронирования.");
            }

            if (product.StockQuantity < dto.Quantity)
            {
                throw new BusinessRuleException(
                    $"Недостаточно товара «{product.ProductName}» на складе: доступно {product.StockQuantity}, запрошено {dto.Quantity}.");
            }

            product.StockQuantity -= dto.Quantity;

            var reservation = _mapper.Map<ProductReservation>(dto);
            reservation.ReservedDate = DateTime.Now;
            await _reservations.AddAsync(reservation, ct);

            await _reservations.SaveChangesAsync(ct);

            var saved = await _reservations.GetByIdAsync(reservation.IdReservation, ct)
                ?? throw new InvalidOperationException($"Бронь с Id {reservation.IdReservation} не найдена.");
            return _mapper.Map<ReservationDTO>(saved);
        }

        public async Task<ReservationDTO?> CancelAsync(int id, CancellationToken ct)
        {
            var reservation = await _reservations.GetActiveByIdAsync(id, ct);
            if (reservation is null)
            {
                return null;
            }

            reservation.Product.StockQuantity += reservation.Quantity;
            reservation.ReleasedDate = DateTime.Now;

            await _reservations.SaveChangesAsync(ct);

            return _mapper.Map<ReservationDTO>(reservation);
        }
    }
}
