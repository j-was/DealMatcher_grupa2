class OfferCreate {
  final String _title;
  final String _description;
  final double _price;
  final List<String> _images;
  final int _categoryId;
  final List<String> _tags;
  final Map<String, String> _properties;
  final int _availability;

  String get title => _title;
  String get description => _description;
  double get price => _price;
  List<String> get images => List.unmodifiable(_images);
  int get categoryId => _categoryId;
  List<String> get tags => List.unmodifiable(_tags);
  Map<String, String> get properties => Map.unmodifiable(_properties);
  int get availability => _availability;

  const OfferCreate({
    required String title,
    required String description,
    required double price,
    required List<String> images,
    required int categoryId,
    required List<String> tags,
    required Map<String, String> properties,
    required int availability,
  }) : _title = title,
       _description = description,
       _price = price,
       _images = images,
       _categoryId = categoryId,
       _tags = tags,
       _properties = properties,
       _availability = availability;

  Map<String, dynamic> toJson() => {
    'title': _title,
    'description': _description,
    'price': _price,
    'images': _images,
    'categoryId': _categoryId,
    'tags': _tags,
    'properties': _properties,
    'availability': _availability,
  };
}
