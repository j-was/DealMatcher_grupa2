import 'package:flutter/material.dart';

class AppTheme {
  static const _primaryColor = Color.fromARGB(255, 214, 163, 10);
  static const _backgroundColor = Color.fromARGB(255, 47, 45, 45);

  static ThemeData light() {
    return ThemeData(
      useMaterial3: true,
      scaffoldBackgroundColor: _backgroundColor,
      primaryColor: _primaryColor,
      colorScheme: ColorScheme.fromSeed(seedColor: _primaryColor),
      appBarTheme: const AppBarTheme(
        centerTitle: true,
        elevation: 0,
        backgroundColor: Color.fromARGB(255, 214, 163, 10),
        foregroundColor: Colors.white,
      ),
      drawerTheme: const DrawerThemeData(
        backgroundColor: _primaryColor,
        surfaceTintColor: Colors.transparent,
      ),
      textTheme: const TextTheme(
        titleLarge: TextStyle(fontWeight: FontWeight.bold),
      ),
      listTileTheme: ListTileThemeData(
        textColor: Colors.white,
        iconColor: Colors.white,
      ),
      elevatedButtonTheme: ElevatedButtonThemeData(
        style: ElevatedButton.styleFrom(
          padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 12),
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadiusGeometry.circular(12),
          ),
          backgroundColor: _primaryColor,
        ),
      ),
      inputDecorationTheme: InputDecorationThemeData(
        labelStyle: TextStyle(color: Colors.white54),
        border: OutlineInputBorder(
          borderRadius: BorderRadius.circular(12),
          borderSide: BorderSide(color: Colors.white24),
        ),
        alignLabelWithHint: true,
      ),
      iconButtonTheme: IconButtonThemeData(
        style: ButtonStyle(iconColor: WidgetStatePropertyAll(Colors.white54)),
      ),
    );
  }
}
