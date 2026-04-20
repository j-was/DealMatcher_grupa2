import 'package:flutter/material.dart';
import 'package:frontend/Models/offer.dart';
import 'package:frontend/Services/offer_service.dart';
import 'package:go_router/go_router.dart';

class MyOffersSection extends StatefulWidget {
  const MyOffersSection({super.key});

  @override
  State<MyOffersSection> createState() => _MyOffersSectionState();
}

class _MyOffersSectionState extends State<MyOffersSection> {
  final OfferService _offerService = OfferService();
  late Future<List<Offer>> _futureOffers;

  @override
  void initState() {
    super.initState();
    _futureOffers = _offerService.getMyOffers();
  }

  void _reload() {
    setState(() {
      _futureOffers = _offerService.getMyOffers();
    });
  }

  @override
  Widget build(BuildContext context) {
    final primary = Theme.of(context).primaryColor;

    return FutureBuilder<List<Offer>>(
      future: _futureOffers,
      builder: (context, snapshot) {
        if (snapshot.connectionState == ConnectionState.waiting) {
          return const Center(
            child: Padding(
              padding: EdgeInsets.all(32),
              child: CircularProgressIndicator(),
            ),
          );
        }

        if (snapshot.hasError) {
          return _EmptyState(
            icon: Icons.error_outline,
            title: 'Nie udało się pobrać ofert',
            subtitle: snapshot.error.toString(),
            actionLabel: 'Spróbuj ponownie',
            onAction: _reload,
          );
        }

        final offers = snapshot.data ?? [];

        return ListView(
          children: [
            _Header(primary: primary),
            const SizedBox(height: 18),
            if (offers.isEmpty)
              _EmptyState(
                icon: Icons.inbox_outlined,
                title: 'Nie masz jeszcze żadnych ofert',
                subtitle: 'Dodaj pierwszą ofertę, a pojawi się tutaj.',
                actionLabel: 'Odśwież',
                onAction: _reload,
              )
            else
              ...offers.map((offer) {
                return Padding(
                  padding: const EdgeInsets.only(bottom: 14),
                  child: _OfferCard(
                    offer: offer,
                    onTap: () => context.go('/my-offers/${offer.id}'),
                  ),
                );
              }),
          ],
        );
      },
    );
  }
}

class _Header extends StatelessWidget {
  final Color primary;
  const _Header({required this.primary});

  @override
  Widget build(BuildContext context) {
    return Container(
      decoration: BoxDecoration(
        borderRadius: BorderRadius.circular(24),
        gradient: LinearGradient(
          colors: [
            primary.withValues(alpha: 0.22),
            const Color.fromARGB(255, 28, 28, 28),
          ],
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        border: Border.all(color: primary.withValues(alpha: 0.25)),
        boxShadow: [
          BoxShadow(
            blurRadius: 24,
            offset: const Offset(0, 10),
            color: Colors.black.withValues(alpha: 0.25),
          ),
        ],
      ),
      padding: const EdgeInsets.all(20),
      child: Row(
        children: [
          CircleAvatar(
            radius: 28,
            backgroundColor: primary,
            child: const Icon(Icons.sell_outlined, color: Colors.white),
          ),
          const SizedBox(width: 16),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  'Moje oferty',
                  style: Theme.of(context).textTheme.titleLarge?.copyWith(
                    color: Colors.white,
                    fontSize: 24,
                  ),
                ),
                const SizedBox(height: 4),
                const Text(
                  'Kliknij w kartę, aby przejść do edycji.',
                  style: TextStyle(color: Colors.white70),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}

class _OfferCard extends StatelessWidget {
  final Offer offer;
  final VoidCallback onTap;

  const _OfferCard({required this.offer, required this.onTap});

  String _formatDate(DateTime date) {
    final d = date.day.toString().padLeft(2, '0');
    final m = date.month.toString().padLeft(2, '0');
    return '$d.$m.${date.year}';
  }

  @override
  Widget build(BuildContext context) {
    final primary = Theme.of(context).primaryColor;
    final image = offer.images.isNotEmpty ? offer.images.first : null;

    return Container(
      decoration: BoxDecoration(
        borderRadius: BorderRadius.circular(22),
        gradient: const LinearGradient(
          colors: [
            Color.fromARGB(255, 40, 37, 34),
            Color.fromARGB(255, 25, 25, 25),
          ],
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        border: Border.all(color: primary.withValues(alpha: 0.18)),
        boxShadow: [
          BoxShadow(
            blurRadius: 16,
            offset: const Offset(0, 8),
            color: Colors.black.withValues(alpha: 0.22),
          ),
        ],
      ),
      child: Material(
        color: Colors.transparent,
        child: InkWell(
          borderRadius: BorderRadius.circular(22),
          onTap: onTap,
          child: Padding(
            padding: const EdgeInsets.all(16),
            child: Row(
              crossAxisAlignment: CrossAxisAlignment.center,
              children: [
                // Thumbnail
                _Thumbnail(image: image),
                const SizedBox(width: 16),

                // Title + meta info
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        offer.title,
                        style: const TextStyle(
                          color: Colors.white,
                          fontSize: 16,
                          fontWeight: FontWeight.w700,
                        ),
                        maxLines: 2,
                        overflow: TextOverflow.ellipsis,
                      ),
                      const SizedBox(height: 10),
                      Wrap(
                        spacing: 8,
                        runSpacing: 6,
                        children: [
                          _MetaPill(
                            icon: Icons.inventory_2_outlined,
                            label: 'Szt: ${offer.availability}',
                          ),
                          _MetaPill(
                            icon: Icons.calendar_today_outlined,
                            label: _formatDate(offer.createdAt),
                          ),
                        ],
                      ),
                    ],
                  ),
                ),
                const SizedBox(width: 12),

                _PriceBadge(price: offer.price, primary: primary),
              ],
            ),
          ),
        ),
      ),
    );
  }
}

class _Thumbnail extends StatelessWidget {
  final String? image;
  const _Thumbnail({required this.image});

  @override
  Widget build(BuildContext context) {
    return ClipRRect(
      borderRadius: BorderRadius.circular(14),
      child: Container(
        width: 80,
        height: 80,
        color: Colors.white.withValues(alpha: 0.05),
        child: image == null
            ? const Icon(
                Icons.image_not_supported_outlined,
                color: Colors.white38,
              )
            : Image.network(
                image!,
                fit: BoxFit.cover,
                errorBuilder: (_, __, ___) => const Icon(
                  Icons.image_not_supported_outlined,
                  color: Colors.white38,
                ),
              ),
      ),
    );
  }
}

class _PriceBadge extends StatelessWidget {
  final double price;
  final Color primary;
  const _PriceBadge({required this.price, required this.primary});

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
      decoration: BoxDecoration(
        color: primary.withValues(alpha: 0.16),
        borderRadius: BorderRadius.circular(999),
        border: Border.all(color: primary.withValues(alpha: 0.35)),
      ),
      child: Text(
        '${price.toStringAsFixed(2)} zł',
        style: const TextStyle(
          color: Colors.white,
          fontWeight: FontWeight.w700,
          fontSize: 15,
        ),
      ),
    );
  }
}

class _MetaPill extends StatelessWidget {
  final IconData icon;
  final String label;
  const _MetaPill({required this.icon, required this.label});

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
      decoration: BoxDecoration(
        color: Colors.white.withValues(alpha: 0.05),
        borderRadius: BorderRadius.circular(8),
        border: Border.all(color: Colors.white.withValues(alpha: 0.08)),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Icon(icon, size: 13, color: Colors.white54),
          const SizedBox(width: 5),
          Text(
            label,
            style: const TextStyle(color: Colors.white60, fontSize: 12),
          ),
        ],
      ),
    );
  }
}

class _EmptyState extends StatelessWidget {
  final IconData icon;
  final String title;
  final String subtitle;
  final String actionLabel;
  final VoidCallback onAction;

  const _EmptyState({
    required this.icon,
    required this.title,
    required this.subtitle,
    required this.actionLabel,
    required this.onAction,
  });

  @override
  Widget build(BuildContext context) {
    final primary = Theme.of(context).primaryColor;

    return Container(
      margin: const EdgeInsets.only(top: 18),
      padding: const EdgeInsets.all(24),
      decoration: BoxDecoration(
        borderRadius: BorderRadius.circular(22),
        color: Colors.white.withValues(alpha: 0.04),
        border: Border.all(color: primary.withValues(alpha: 0.18)),
      ),
      child: Column(
        children: [
          Icon(icon, size: 48, color: primary),
          const SizedBox(height: 14),
          Text(
            title,
            style: const TextStyle(
              color: Colors.white,
              fontSize: 18,
              fontWeight: FontWeight.w700,
            ),
            textAlign: TextAlign.center,
          ),
          const SizedBox(height: 8),
          Text(
            subtitle,
            style: const TextStyle(color: Colors.white70),
            textAlign: TextAlign.center,
          ),
          const SizedBox(height: 18),
          ElevatedButton.icon(
            onPressed: onAction,
            icon: const Icon(Icons.refresh, size: 18),
            label: Text(actionLabel),
          ),
        ],
      ),
    );
  }
}
