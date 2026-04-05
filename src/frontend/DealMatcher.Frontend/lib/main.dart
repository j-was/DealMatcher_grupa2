import 'package:flutter/material.dart';
import 'package:frontend/Router/router.dart';
import 'package:frontend/Themes/app_theme.dart';
import 'package:url_strategy/url_strategy.dart';

void main() {
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
