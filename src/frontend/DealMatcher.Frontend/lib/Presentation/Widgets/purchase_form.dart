import 'package:flutter/material.dart';
import 'package:frontend/Models/cart_item.dart';
import 'package:frontend/Models/delivery_method.dart';
import 'package:frontend/Models/payment_method.dart';
import 'package:frontend/Models/purchase_request.dart';
import 'package:frontend/Services/purchase_service.dart';
import 'package:go_router/go_router.dart';
import 'package:url_launcher/url_launcher.dart';

final List<DeliveryMethod> mockDeliveryMethods = [
  DeliveryMethod(
    id: 'd1',
    name: 'Kurier',
    price: 15.0,
    estimatedDays: 2,
    description: '',
  ),
  DeliveryMethod(
    id: 'd2',
    name: 'Paczkomat',
    price: 10.0,
    estimatedDays: 1,
    description: '',
  ),
];

final List<PaymentMethod> mockPaymentMethods = [
  PaymentMethod(id: 'p1', name: 'BLIK', provider: '', icon: ''),
  PaymentMethod(id: 'p2', name: 'Karta', provider: '', icon: ''),
];

class PurchaseForm extends StatefulWidget {
  final List<CartItem> cartItems;

  const PurchaseForm({super.key, required this.cartItems});

  @override
  State<PurchaseForm> createState() => _PurchaseFormState();
}

class _PurchaseFormState extends State<PurchaseForm> {
  final _purchaseService = PurchaseService.instance;
  final _formKey = GlobalKey<FormState>();

  late final Future<void> _loadFuture;

  List<DeliveryMethod> _deliveryMethods = <DeliveryMethod>[];
  List<PaymentMethod> _paymentMethods = <PaymentMethod>[];

  String? _selectedDeliveryId;
  String? _selectedPaymentId;
  bool _isSubmitting = false;

  @override
  void initState() {
    super.initState();
    _loadFuture = _loadMethods();
  }

  double _totalForItem(CartItem item) {
    return item.offer.price * item.quantity;
  }

  double _totalForCart() {
    return widget.cartItems.fold<double>(
      0.0,
      (sum, item) => sum + _totalForItem(item),
    );
  }

  Future<void> _loadMethods() async {
    final results = await Future.wait([
      _purchaseService.getDeliveryMethods(),
      _purchaseService.getPaymentMethods(),
    ]);

    if (!mounted) return;

    setState(() {
      _deliveryMethods = results[0] as List<DeliveryMethod>;
      _paymentMethods = results[1] as List<PaymentMethod>;

      if (_deliveryMethods.isNotEmpty) {
        _selectedDeliveryId = _deliveryMethods.first.id;
      }
      if (_paymentMethods.isNotEmpty) {
        _selectedPaymentId = _paymentMethods.first.id;
      }
    });
  }

  Future<Uri> _submitSingleItem(
    CartItem item,
    String deliveryMethodId,
    String paymentMethodId,
  ) async {
    final request = PurchaseRequest(
      offerId: item.offer.id,
      deliveryMethodId: deliveryMethodId,
      paymentMethodId: paymentMethodId,
      quantity: item.quantity,
    );

    return await _purchaseService.initializePurchase(request);
  }

  Future<void> _submitAll() async {
    if (!(_formKey.currentState?.validate() ?? false)) return;

    final deliveryId = _selectedDeliveryId!;
    final paymentId = _selectedPaymentId!;

    setState(() => _isSubmitting = true);

    final List<String> success = [];
    final List<String> failed = [];
    Uri? redirectUri;

    for (final item in widget.cartItems) {
      try {
        final uri = await _submitSingleItem(item, deliveryId, paymentId);

        redirectUri ??= uri;

        success.add(item.offer.title);
      } catch (e) {
        failed.add(item.offer.title);
      }
    }

    if (!mounted) return;

    setState(() => _isSubmitting = false);

    if (failed.isEmpty) {
      if (redirectUri != null) {
        final launched = await launchUrl(
          redirectUri,
          mode: LaunchMode.externalApplication,
        );

        if (!mounted) return;

        if (!launched) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(
              content: Text('Nie udało się otworzyć strony płatności.'),
            ),
          );
          return;
        }
      } else {
        context.go("/delivery");
      }

      ScaffoldMessenger.of(
        context,
      ).showSnackBar(const SnackBar(content: Text('Złożono zamówienie')));
    } else {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text(
            'Udało się: ${success.length}, nie udało się: ${failed.length}',
          ),
        ),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return FutureBuilder<void>(
      future: _loadFuture,
      builder: (context, snapshot) {
        if (snapshot.connectionState == ConnectionState.waiting &&
            _deliveryMethods.isEmpty &&
            _paymentMethods.isEmpty) {
          return const Padding(
            padding: EdgeInsets.symmetric(vertical: 24),
            child: Center(child: CircularProgressIndicator()),
          );
        }

        if (snapshot.hasError) {
          return Padding(
            padding: const EdgeInsets.all(16),
            child: Text(
              'Nie udało się wczytać danych zamówienia: ${snapshot.error}',
            ),
          );
        }

        if (widget.cartItems.isEmpty) {
          return const Card(
            child: Padding(
              padding: EdgeInsets.all(16),
              child: Text('Koszyk jest pusty.'),
            ),
          );
        }

        return SingleChildScrollView(
          child: Card(
            child: Padding(
              padding: const EdgeInsets.all(16),
              child: Form(
                key: _formKey,
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: [
                    Text(
                      'Podsumowanie zamówienia',
                      style: Theme.of(context).textTheme.titleMedium?.copyWith(
                        fontWeight: FontWeight.w700,
                      ),
                    ),
                    const SizedBox(height: 12),

                    ...widget.cartItems.map(
                      (item) => Padding(
                        padding: const EdgeInsets.only(bottom: 10),
                        child: Row(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Expanded(
                              child: Text(
                                item.offer.title,
                                maxLines: 2,
                                overflow: TextOverflow.ellipsis,
                                style: const TextStyle(
                                  fontWeight: FontWeight.w600,
                                ),
                              ),
                            ),
                            const SizedBox(width: 12),
                            Text(
                              '${item.quantity} x ${item.offer.price.toStringAsFixed(2)} zł',
                              style: const TextStyle(
                                fontWeight: FontWeight.w500,
                              ),
                            ),
                          ],
                        ),
                      ),
                    ),

                    const Divider(height: 24),

                    Text(
                      'Razem: ${_totalForCart().toStringAsFixed(2)} zł',
                      style: const TextStyle(
                        fontSize: 16,
                        fontWeight: FontWeight.bold,
                      ),
                    ),

                    const SizedBox(height: 20),

                    DropdownButtonFormField<String>(
                      initialValue: _selectedDeliveryId,
                      decoration: const InputDecoration(
                        labelText: 'Metoda dostawy',
                      ),
                      items: _deliveryMethods
                          .map(
                            (method) => DropdownMenuItem<String>(
                              value: method.id,
                              child: Text(
                                '${method.name} • ${method.price.toStringAsFixed(2)} zł',
                                overflow: TextOverflow.ellipsis,
                              ),
                            ),
                          )
                          .toList(),
                      onChanged: (value) {
                        setState(() {
                          _selectedDeliveryId = value;
                        });
                      },
                      validator: (value) {
                        if (value == null) {
                          return 'Wybierz metodę dostawy';
                        }
                        return null;
                      },
                    ),

                    const SizedBox(height: 12),

                    DropdownButtonFormField<String>(
                      initialValue: _selectedPaymentId,
                      decoration: const InputDecoration(
                        labelText: 'Metoda płatności',
                      ),
                      items: _paymentMethods
                          .map(
                            (method) => DropdownMenuItem<String>(
                              value: method.id,
                              child: Text(
                                method.name,
                                overflow: TextOverflow.ellipsis,
                              ),
                            ),
                          )
                          .toList(),
                      onChanged: (value) {
                        setState(() {
                          _selectedPaymentId = value;
                        });
                      },
                      validator: (value) {
                        if (value == null) {
                          return 'Wybierz metodę płatności';
                        }
                        return null;
                      },
                    ),

                    const SizedBox(height: 20),

                    Center(
                      child: SizedBox(
                        width: 220,
                        child: ElevatedButton(
                          onPressed: _isSubmitting ? null : _submitAll,
                          child: _isSubmitting
                              ? const SizedBox(
                                  width: 20,
                                  height: 20,
                                  child: CircularProgressIndicator(
                                    strokeWidth: 2,
                                  ),
                                )
                              : const Text('Potwierdź zamówienie'),
                        ),
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ),
        );
      },
    );
  }
}
