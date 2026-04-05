import 'package:flutter/material.dart';

class SeparatedWidget extends StatelessWidget {
  final Widget widget;
  final double height;
  const SeparatedWidget({super.key, required this.widget, this.height = 50});
  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: EdgeInsets.only(bottom: height, left: 20, right: 20),
      child: widget,
    );
  }
}
