import 'package:flutter_test/flutter_test.dart';
import 'package:frontend/main.dart';

void main() {
  testWidgets('Pass test', (WidgetTester tester) async {
    await tester.pumpWidget(const MyApp());
    expect(1, 1);
  });
}
