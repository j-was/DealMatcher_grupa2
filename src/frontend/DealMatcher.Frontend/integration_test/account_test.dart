import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:integration_test/integration_test.dart';
import 'package:frontend/Services/auth_service.dart';
import 'package:frontend/main.dart' as app;

void main() {
  IntegrationTestWidgetsFlutterBinding.ensureInitialized();
  setUp(() async {
  });

  testWidgets('Account test: Register, login, change name, delete',
      (WidgetTester tester) async {
    final timestamp = DateTime.now().millisecondsSinceEpoch;
    final testEmail = 'e2e_$timestamp@test.com';
    final testPassword = 'Test.1234';
    final testName = 'John';
    final testSurname = 'Doe';
    final newName = 'Adam';

    await tester.runAsync(() async {
      await AuthService.instance.clearSession();
    });

    app.main();
    await tester.pumpAndSettle(const Duration(seconds: 3));

    final scaffold = find.byType(Scaffold);
    expect(scaffold, findsOneWidget);

    await tester.tap(find.byTooltip('Open navigation menu'));
    await tester.pumpAndSettle();

    final registerTile = find.text('Zarejestruj się');
    expect(registerTile, findsOneWidget);
    await tester.tap(registerTile);
    await tester.pumpAndSettle(const Duration(seconds: 2));

    expect(find.text('Wyślij'), findsOneWidget); 

    await tester.enterText(find.byKey(const Key('nameField')), testName);
    await tester.enterText(
        find.byKey(const Key('lastNameField')), testSurname);
    await tester.enterText(find.byKey(const Key('emailField')), testEmail);
    await tester.enterText(
        find.byKey(const Key('passwordField')), testPassword);
    await tester.enterText(
        find.byKey(const Key('confirmPasswordField')), testPassword);

    await tester.tap(find.byKey(const Key('signUpSubmitButton')));
    await tester.pumpAndSettle(const Duration(seconds: 5));

    expect(find.byKey(const Key('loginSubmitButton')), findsOneWidget);

    await tester.enterText(
        find.byKey(const Key('loginEmailField')), testEmail);
    await tester.enterText(
        find.byKey(const Key('loginPasswordField')), testPassword);

    await tester.tap(find.byKey(const Key('loginSubmitButton')));
    await tester.pumpAndSettle(const Duration(seconds: 5));

    expect(find.byKey(const Key('profileNameField')), findsOneWidget);
    expect(find.byKey(const Key('profileSurnameField')), findsOneWidget);

    final nameField =
        tester.widget<TextFormField>(find.byKey(const Key('profileNameField')));
    expect(nameField.controller?.text, testName);

    await tester.enterText(find.byKey(const Key('profileNameField')), '');
    await tester.enterText(
        find.byKey(const Key('profileNameField')), newName);

    await tester.tap(find.byKey(const Key('profileSaveButton')));
    await tester.pumpAndSettle(const Duration(seconds: 3));

    expect(find.text('Profil zapisany'), findsOneWidget);

    final deleteText = find.text('Usuń konto');
    await tester.ensureVisible(deleteText.last);
    await tester.pumpAndSettle();
    
    await tester.tap(deleteText.last);
    await tester.pumpAndSettle();

    await tester.tap(find.text('Usuń').last);
    await tester.pumpAndSettle(const Duration(seconds: 5));

    expect(find.byKey(const Key('loginSubmitButton')), findsOneWidget);
  });
}