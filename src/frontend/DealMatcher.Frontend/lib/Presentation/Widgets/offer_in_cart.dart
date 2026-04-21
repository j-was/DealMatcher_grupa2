import 'package:flutter/material.dart';
import 'package:frontend/Models/cart_item.dart';

class OfferInCart extends StatefulWidget {
  final CartItem cartItem;
  final Future<void> Function(int cartItemId, int quantity) onQuantityChanged;
  final Future<void> Function(int cartItemId) onDelete;

  const OfferInCart({
    super.key,
    required this.cartItem,
    required this.onQuantityChanged,
    required this.onDelete,
  });

  @override
  State<OfferInCart> createState() => _OfferInCartState();
}

class _OfferInCartState extends State<OfferInCart> {
  late int _quantity;

  @override
  void initState() {
    super.initState();
    _quantity = widget.cartItem.quantity;
  }

  void increaseQuantity() {
    if (_quantity < widget.cartItem.offer.availability) {
      setState(() {
        _quantity++;
      });
    }
  }

  void decreaseQuantity() {
    if (_quantity > 1) {
      setState(() {
        _quantity--;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    final offer = widget.cartItem.offer;
    final double totalPrice = offer.price * _quantity;

    return Container(
      height: 150,
      padding: const EdgeInsets.all(14),
      decoration: BoxDecoration(
        color: Theme.of(context).primaryColor,
        borderRadius: BorderRadius.circular(20),
      ),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          ClipRRect(
            borderRadius: BorderRadius.circular(16),
            child: SizedBox(
              width: 100,
              height: 100,
              child: offer.images.isNotEmpty
                  ? Image.network(offer.images[0], fit: BoxFit.cover)
                  : Container(color: Colors.grey.shade200),
            ),
          ),
          const SizedBox(width: 16),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Row(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Expanded(
                      child: Text(
                        offer.title,
                        maxLines: 2,
                        overflow: TextOverflow.ellipsis,
                        style: const TextStyle(
                          fontSize: 16,
                          fontWeight: FontWeight.w600,
                        ),
                      ),
                    ),
                    SizedBox(
                      width: 24,
                      height: 24,
                      child: IconButton(
                        padding: EdgeInsets.all(0),
                        iconSize: 20,
                        onPressed: () {},
                        icon: const Icon(Icons.delete_outline),
                        color: Colors.grey,
                      ),
                    ),
                  ],
                ),
                const SizedBox(height: 8),
                Text(
                  '$totalPrice zł (${offer.price} zł x $_quantity)',
                  style: const TextStyle(
                    fontSize: 14,
                    fontWeight: FontWeight.bold,
                  ),
                ),
                const SizedBox(height: 24),
                Row(
                  children: [
                    Text('Ilość:'),
                    const SizedBox(width: 12),
                    Container(
                      decoration: BoxDecoration(
                        border: Border.all(color: Colors.black45),
                        borderRadius: BorderRadius.circular(12),
                      ),
                      child: Row(
                        children: [
                          IconButton(
                            onPressed: decreaseQuantity,
                            icon: const Icon(
                              Icons.remove,
                              color: Colors.black45,
                            ),
                            visualDensity: VisualDensity.compact,
                          ),
                          Text(
                            '$_quantity',
                            style: Theme.of(context).textTheme.titleSmall,
                          ),
                          IconButton(
                            onPressed: increaseQuantity,
                            icon: const Icon(Icons.add, color: Colors.black45),
                            visualDensity: VisualDensity.compact,
                          ),
                        ],
                      ),
                    ),
                  ],
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}
