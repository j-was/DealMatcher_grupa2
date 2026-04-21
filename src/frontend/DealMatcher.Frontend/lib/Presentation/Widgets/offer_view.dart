import 'package:flutter/material.dart';
import 'package:frontend/Models/offer.dart';
import 'package:frontend/Services/cart_service.dart';
import 'package:go_router/go_router.dart';

class OfferView extends StatefulWidget {
  final Offer offer;

  const OfferView({super.key, required this.offer});

  @override
  State createState() => OfferViewState();
}

class OfferViewState extends State<OfferView> {
  bool ifExpanded = false;
  bool _isAdding = false;

  Future<void> _addToCart(BuildContext context) async {
    if (_isAdding) return;

    setState(() {
      _isAdding = true;
    });

    try {
      await CartService.instance.addToCart(offerId: widget.offer.id);

      if (!context.mounted) return;
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(const SnackBar(content: Text('Dodano do koszyka')));
    } on StateError {
      if (!context.mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Zaloguj się, aby dodać do koszyka')),
      );
      context.go('/login');
    } catch (e) {
      if (!context.mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Nie udało się dodać do koszyka: $e')),
      );
    } finally {
      if (mounted) {
        setState(() {
          _isAdding = false;
        });
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    final hasImage = widget.offer.images.isNotEmpty;

    return Center(
      child: SizedBox(
        width: 400,
        height: ifExpanded ? 610 : 425,
        child: InkWell(
          onTap: () {
            setState(() {
              ifExpanded = !ifExpanded;
            });
          },
          child: Card(
            clipBehavior: Clip.antiAlias,
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadiusGeometry.circular(16),
            ),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                if (hasImage)
                  SizedBox(
                    height: 220,
                    width: double.infinity,
                    child: Image.network(
                      widget.offer.images[0],
                      fit: BoxFit.cover,
                      errorBuilder: (context, error, stackTrace) {
                        return Container(
                          color: Colors.grey.shade300,
                          child: const Center(
                            child: Icon(Icons.broken_image_outlined),
                          ),
                        );
                      },
                    ),
                  ),
                Expanded(
                  child: ListView(
                    children: [
                      Padding(
                        padding: const EdgeInsets.all(12),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text(
                              widget.offer.title,
                              style: const TextStyle(
                                fontWeight: FontWeight.bold,
                                fontSize: 20,
                              ),
                            ),
                            Text('${widget.offer.price.toStringAsFixed(2)} zł'),
                            const SizedBox(height: 12),
                            Wrap(
                              spacing: 6,
                              children: widget.offer.tags
                                  .map((tag) => Chip(label: Text(tag)))
                                  .toList(),
                            ),
                            const SizedBox(height: 8),
                            Text(
                              widget.offer.description,
                              maxLines: ifExpanded ? null : 2,
                              overflow: ifExpanded
                                  ? TextOverflow.visible
                                  : TextOverflow.ellipsis,
                            ),
                          ],
                        ),
                      ),
                      if (ifExpanded) ...[
                        Padding(
                          padding: const EdgeInsets.fromLTRB(12, 0, 12, 12),
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              const SizedBox(height: 8),
                              ...widget.offer.properties.map(
                                (property) => Row(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Text('${property.$1}: '),
                                    Expanded(child: Text(property.$2)),
                                  ],
                                ),
                              ),
                              const SizedBox(height: 8),
                              Text('Ilość: ${widget.offer.availability}'),
                              const SizedBox(height: 16),
                              SizedBox(
                                width: double.infinity,
                                child: ElevatedButton.icon(
                                  onPressed:
                                      widget.offer.availability <= 0 ||
                                          _isAdding
                                      ? null
                                      : () => _addToCart(context),
                                  icon: _isAdding
                                      ? const SizedBox(
                                          width: 16,
                                          height: 16,
                                          child: CircularProgressIndicator(
                                            strokeWidth: 2,
                                          ),
                                        )
                                      : const Icon(
                                          Icons.shopping_cart_outlined,
                                        ),
                                  label: Text(
                                    widget.offer.availability <= 0
                                        ? 'Brak w magazynie'
                                        : 'Dodaj do koszyka',
                                  ),
                                ),
                              ),
                              const SizedBox(height: 12),
                              Text(
                                'Utworzono: ${widget.offer.createdAt.toString().split(' ')[0]}',
                                style: const TextStyle(fontSize: 12),
                              ),
                            ],
                          ),
                        ),
                      ],
                    ],
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
