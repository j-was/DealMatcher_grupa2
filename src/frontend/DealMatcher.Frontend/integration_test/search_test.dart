import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:integration_test/integration_test.dart';
import 'package:frontend/Services/auth_service.dart';
import 'package:frontend/main.dart' as app;

void main() {
  IntegrationTestWidgetsFlutterBinding.ensureInitialized();

  testWidgets('Search test: tag and price, verify result',
      (WidgetTester tester) async {
    await tester.runAsync(() async {
      await AuthService.instance.clearSession();
    });

    app.main();
    await tester.pumpAndSettle(const Duration(seconds: 3));

    await tester.tap(find.byTooltip('Open navigation menu'));
    await tester.pumpAndSettle();

    await tester.tap(find.text('Szukaj'));
    await tester.pumpAndSettle(const Duration(seconds: 2));

    expect(find.byKey(const Key('searchSubmitButton')), findsOneWidget);

    final tagField = find.byKey(const Key('tagField0'));
    await tester.enterText(tagField, 'sport');
    await tester.pumpAndSettle();

    final minPriceField = find.widgetWithText(TextFormField, 'Min. cena');
    await tester.enterText(minPriceField, '200');
    await tester.pumpAndSettle();

    final maxPriceField = find.widgetWithText(TextFormField, 'Max. cena');
    await tester.enterText(maxPriceField, '500');
    await tester.pumpAndSettle();

    await tester.tap(find.byKey(const Key('searchSubmitButton')));
    await tester.pumpAndSettle(const Duration(seconds: 5));

    expect(
      find.byKey(const Key('searchSubmitButton')),
      findsNothing,
    );

    final noOffers = find.text('Brak ofert');
    if (noOffers.evaluate().isNotEmpty) {
      expect(noOffers, findsOneWidget);
      return;
    }

    expect(find.byType(Chip), findsWidgets);

    final chips = find.byType(Chip);
    var foundSportTag = false;
    for (final element in chips.evaluate()) {
      final chip = element.widget as Chip;
      final label = chip.label;
      if (label is Text && label.data?.toLowerCase() == 'sport') {
        foundSportTag = true;
        break;
      }
    }
    expect(foundSportTag, isTrue, reason: 'No offer with "sport" tag found');

    var moreOffers = true;
    while (moreOffers) {
      final currentChips = find.byType(Chip);
      if (currentChips.evaluate().isNotEmpty) {
        expect(currentChips, findsWidgets);
      }

      final dislike = find.byIcon(Icons.close);
      if (dislike.evaluate().isEmpty) break;

      await tester.tap(dislike);
      await tester.pumpAndSettle(const Duration(seconds: 1));

      if (find.text('Brak ofert').evaluate().isNotEmpty) {
        moreOffers = false;
      }
    }
  });
}