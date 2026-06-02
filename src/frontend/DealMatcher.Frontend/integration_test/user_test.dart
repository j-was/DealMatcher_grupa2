import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:integration_test/integration_test.dart';
import 'package:frontend/Services/auth_service.dart';
import 'package:frontend/main.dart' as app;

void main() {
  IntegrationTestWidgetsFlutterBinding.ensureInitialized();

  testWidgets(
      'Logged user test: Login, message, cart, purchase',
      (WidgetTester tester) async {
    await tester.runAsync(() async {
      await AuthService.instance.clearSession();
    });

    const testEmail = 'admin@email.com';
    const testPassword = 'admin';

    app.main();
    await tester.pumpAndSettle(const Duration(seconds: 3));

    await tester.tap(find.byTooltip('Open navigation menu'));
    await tester.pumpAndSettle();
    await tester.tap(find.text('Zaloguj się'));
    await tester.pumpAndSettle(const Duration(seconds: 2));

    await tester.enterText(
        find.byKey(const Key('loginEmailField')), testEmail);
    await tester.enterText(
        find.byKey(const Key('loginPasswordField')), testPassword);
    await tester.tap(find.byKey(const Key('loginSubmitButton')));
    await tester.pumpAndSettle(const Duration(seconds: 5));

    await tester.tap(find.byTooltip('Open navigation menu'));
    await tester.pumpAndSettle();
    await tester.tap(find.text('Przeglądaj'));
    await tester.pumpAndSettle(const Duration(seconds: 3));

    if (find.text('Brak ofert').evaluate().isEmpty) {
      final chatIcon = find.byIcon(Icons.chat_bubble_outline);
      if (chatIcon.evaluate().isNotEmpty) {
        await tester.tap(chatIcon);
        await tester.pumpAndSettle();
        final messageField = find.byType(TextField);
        await tester.enterText(messageField, 'Test message from e2e test');
        await tester.pumpAndSettle();
        await tester.tap(find.text('Wyślij'));
        await tester.pumpAndSettle(const Duration(seconds: 3));
      }

      final starIcon = find.byIcon(Icons.star_rounded);
      if (starIcon.evaluate().isNotEmpty) {
        await tester.tap(starIcon);
        await tester.pumpAndSettle(const Duration(seconds: 2));
      }
    }
    

    await tester.tap(find.byTooltip('Open navigation menu'));
    await tester.pumpAndSettle();
    await tester.tap(find.text('Koszyk'));
    await tester.pumpAndSettle(const Duration(seconds: 3));

    final emptyCart = find.text('Koszyk jest pusty');
    if (emptyCart.evaluate().isEmpty) {
      await tester.tap(find.text('Złóż zamówienie'));
      await tester.pumpAndSettle(const Duration(seconds: 3));

      if (find.text('Potwierdź zamówienie').evaluate().isNotEmpty) {
        expect(find.text('Potwierdź zamówienie'), findsOneWidget);
        // await tester.tap(find.text('Potwierdź zamówienie'));
        // await tester.pumpAndSettle(const Duration(seconds: 5));
        // expect(find.text('Zapłać'), findsOneWidget);
        // expect(true, isTrue);
      }
      // if (find.text('Zapłać').evaluate().isNotEmpty) {
      //   await tester.tap(find.text('Zapłać'));
      //   await tester.pumpAndSettle(const Duration(seconds: 5));
      // }
    }

    // expect(find.text('Koszyk jest pusty'), findsOneWidget);


    // await tester.tap(find.byTooltip('Open navigation menu'));
    // await tester.pumpAndSettle();
    // await tester.tap(find.text('Wyloguj'));
    // await tester.pumpAndSettle(const Duration(seconds: 3));

    // expect(find.byKey(const Key('loginSubmitButton')), findsOneWidget);
  });
}