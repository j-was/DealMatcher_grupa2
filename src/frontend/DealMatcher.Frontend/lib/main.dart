import 'package:flutter/material.dart';
import 'package:frontend/Router/router.dart';
import 'package:frontend/Themes/app_theme.dart';
import 'package:url_strategy/url_strategy.dart';
import 'package:frontend/Services/auth_service.dart';

Future<void> main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await AuthService.instance.restoreSession();
  setPathUrlStrategy();
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp.router(
      routerConfig: router,
      title: 'DealMatcher',
      theme: AppTheme.light(),
    );
  }
}
