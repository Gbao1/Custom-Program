# Assignment 2 - House Price Prediction
This project involves analyzing housing price data and predicting house categories (Fixer-Upper, Affordable, Luxury) based on various features. The analysis also includes investigating the correlation between house prices and the number of schools nearby.

## Project Setup
### Environment Configuration
To run this project, you'll need to configure a Python environment. The recommended way to manage dependencies is by using Conda. Follow the steps below to set up the environment:

**1. Create a new conda environment:**

```bash
conda create -n house_price_prediction python=3.9
```
**2. Activate the environment:**

```bash
conda activate house_price_prediction
```

**3. Install required dependencies:**

Make sure to install all the necessary libraries by running the following commands:

```bash
conda install pandas numpy scikit-learn matplotlib seaborn
conda install -c conda-forge jupyterlab
```

**4. Run Jupyter Notebook:**

Navigate to the directory containing your notebook and run:

```bash
jupyter notebook
```
**5. Open the notebook:**

Once the notebook server is running, open the provided notebook file `Assignment 2 ver 2.ipynb` and proceed with the steps inside.

## Data Processing
The notebook handles all the data preprocessing steps, including merging datasets and handling missing values. Here's how to perform the data processing for training:

**1. Dataset Preparation:**

- Ensure that both datasets (housing data and school data) are located in the same directory as the notebook.
- The data will be automatically merged based on the postcode of the houses and schools.
- A new feature, `Schools nearby`, will be added to indicate how many schools are within 500 meters of each house.

**2. Feature Selection:**

- Important features like `Suburb`, `Rooms`, `Distance`, `Landsize`, `BuildingArea`, and `Schools nearby` are used for training the model.
- The target variable is `Price`, and for classification tasks, it has been categorized into three classes: `Fixer-Upper`, `Affordable`, and `Luxury`.

## Model Training
This project uses several models for regression and classification tasks:

**1. Linear Regression:**

The first model is a linear regression used to predict house prices:

```python
lr_model = LinearRegression()
lr_model.fit(x_train, y_train)
```
Evaluate the model using metrics like R-squared and Mean Squared Error:

```python
predictions = lr_model.predict(x_test)
print(r2_score(y_test, predictions))
print(mean_squared_error(y_test, predictions))
```
**2. Random Forest Regression:**

A random forest regression model is also used to predict house prices:

```python
rf_model = RandomForestRegressor()
rf_model.fit(x_train, y_train)
```
Similarly, evaluate the model performance using the same metrics:

```python
predictions_rf = rf_model.predict(x_test)
print(r2_score(y_test, predictions_rf))
print(mean_absolute_error(y_test, predictions_rf))
```
**3. Binary Classification (Price > Median):**

The project also involves a binary classification model to predict if a house's price is above the median:

```python
from sklearn.tree import DecisionTreeClassifier

binary_classifier = DecisionTreeClassifier()
binary_classifier.fit(x_train, y_train_binary)
```
Evaluate the binary classification model:

```python
y_pred = binary_classifier.predict(x_test)
print(accuracy_score(y_test_binary, y_pred))
print(classification_report(y_test_binary, y_pred))
```

**4. Multiclass Classification (Price Category):**

Lastly, a multiclass classification model is used to categorize houses into three categories: `Fixer-Upper`, `Affordable`, and `Luxury`:

```python
dt_classifier = DecisionTreeClassifier()
dt_classifier.fit(x_train, y_train_multiclass)
```
Evaluate the multiclass classification model:

```python
y_pred_multiclass = dt_classifier.predict(x_test)
print(accuracy_score(y_test_multiclass, y_pred_multiclass))
print(classification_report(y_test_multiclass, y_pred_multiclass))
```
## Prediction Guide
Once the models are trained, you can use them to make predictions on new data:

**1. For regression:**

Use the trained regression model to predict house prices:

```python
new_predictions = lr_model.predict(new_data)
```
**2. For classification:**

To predict whether a house falls into a certain price category:

```python
new_class = dt_classifier.predict(new_data)
```
Replace `new_data` with the appropriate data for which you want predictions.

## Performance Evaluation
The notebook includes a comprehensive evaluation of the models, including:

- **Various Charts**: Visualized to better make comparasions.
- **Confusion Matrix**: Visualized using heatmaps to compare the model's predictions with actual values.
- **Accuracy Scores**: To compare model performance on binary and multiclass tasks.
- **Classification Reports**: Display `precision`, `recall`, and `F1-score` for both unmerged and merged datasets, giving insights into model effectiveness.
## Conclusion
This project explores how adding external factors like the number of nearby schools affects house price predictions. The results show some correlation between the number of schools and house prices, but further tuning and data exploration could improve model accuracy.

Feel free to experiment with different features or models to see if performance can be enhanced.
