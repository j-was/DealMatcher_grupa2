import 'package:flutter/material.dart';
import 'package:frontend/Models/offer.dart';
import 'package:frontend/Presentation/Widgets/main_app_bar.dart';
import 'package:frontend/Presentation/Widgets/main_side_menu.dart';
import 'package:frontend/Presentation/Widgets/reaction_buttons.dart';
import 'package:frontend/Presentation/Widgets/searched_offer_section.dart';
import 'package:frontend/Services/cart_service.dart';
import 'package:go_router/go_router.dart';

class SearchedOffersPage extends StatefulWidget {
  final List<Offer> offers;
  const SearchedOffersPage({super.key, required this.offers});
  @override
  State<SearchedOffersPage> createState() => _SearchedOffersPageState();
}

class _SearchedOffersPageState extends State<SearchedOffersPage> {
  int currIdx = 0;
  bool _isAddingToCart = false;
  void nextOffer() {
    if (currIdx < widget.offers.length) {
      setState(() {
        currIdx++;
      });
    }
  }

  Future<void> _addToCart() async {
    if (currIdx >= widget.offers.length || _isAddingToCart) return;
    final offer = widget.offers[currIdx];

    setState(() {
      _isAddingToCart = true;
    });

    try {
      await CartService.instance.addToCart(offerId: offer.id);
      if (!mounted) return;
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(const SnackBar(content: Text('Dodano do koszyka')));
    } on StateError {
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Zaloguj się, aby dodać do koszyka')),
      );
      context.go('/login');
    } catch (e) {
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Nie udało się dodać do koszyka: $e')),
      );
    } finally {
      if (mounted) {
        setState(() {
          _isAddingToCart = false;
        });
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: const MainAppBar(),
      drawer: const MainSideMenu(),
      body: Padding(
        padding: EdgeInsetsGeometry.all(24),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            if (currIdx < widget.offers.length)
              Expanded(
                child: SearchedOfferSection(offer: widget.offers[currIdx]),
              )
            else
              Expanded(
                child: Center(
                  child: Text(
                    'Brak ofert',
                    style: const TextStyle(color: Colors.white54, fontSize: 32),
                  ),
                ),
              ),
            ReactionButtons(
              onDislike: nextOffer,
              onLike: nextOffer,
              onSuperlike: currIdx < widget.offers.length ? _addToCart : null,
            ),
          ],
        ),
      ),
    );
  }
}
