import 'package:flutter/material.dart';
import 'package:frontend/Presentation/Widgets/main_app_bar.dart';
import 'package:go_router/go_router.dart';

class PaymentPage extends StatefulWidget {
  final String paymentMethodId;
  final double price;

  const PaymentPage({
    super.key,
    required this.paymentMethodId,
    required this.price,
  });

  @override
  State<PaymentPage> createState() => _PaymentPageState();
}

class _PaymentPageState extends State<PaymentPage> {
  bool _isLoading = false;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: MainAppBar(),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            const Text(
              'Podsumowanie transakcji',
              style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
            ),
            Text(
              'Łączna kwota: ${widget.price.toStringAsFixed(2)} zł',
              style: TextStyle(fontSize: 16),
            ),
            Text(
              'Metoda platności: ${widget.paymentMethodId}',
              style: TextStyle(fontSize: 16),
            ),
            const Spacer(),
            SizedBox(
              width: double.infinity,
              child: ElevatedButton(
                onPressed: _isLoading
                    ? null
                    : () async {
                        setState(() {
                          _isLoading = true;
                        });

                        await Future.delayed(const Duration(seconds: 2));

                        if (!context.mounted) {
                          return;
                        }

                        context.push('/cart');
                      },
                child: const Text('Zapłać'),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
