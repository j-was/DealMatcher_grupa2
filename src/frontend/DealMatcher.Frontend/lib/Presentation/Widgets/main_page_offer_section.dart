import 'package:flutter/material.dart';
import 'package:frontend/Models/offer.dart';
import 'package:frontend/Presentation/Widgets/offer_view.dart';
import 'package:frontend/Services/offer_service.dart';

class MainPageOfferSection extends StatelessWidget {
  final int offerId;
  final void Function(Offer)? onOfferLoaded;

  const MainPageOfferSection({
    super.key,
    required this.offerId,
    this.onOfferLoaded,
  });

  @override
  Widget build(BuildContext context) {
    final OfferService offerService = OfferService();

    return FutureBuilder<Offer>(
      future: offerService.getOffer(offerId),
      builder: (context, snapshot) {
        if (snapshot.hasError) {
          return Center(child: Text('Błąd pobierania'));
        }

        final offer = snapshot.data;
        if (offer == null) {
          return Center(child: Text('Brak ofert'));
        }
        WidgetsBinding.instance.addPostFrameCallback((_) {
          onOfferLoaded?.call(offer);
        });
        return SingleChildScrollView(child: OfferView(offer: offer));
      },
    );
  }
}
