import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './App.css';

function App() {
  const [foodSales, setFoodSales] = useState([]);
  const [search, setSearch] = useState("");
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");

  useEffect(() => {
    axios.get('http://localhost:5000/api/FoodSales')
      .then(response => setFoodSales(response.data))
      .catch(error => console.error(error));
  }, []);

  // Handle filtering by period date
  const filterByDate = () => {
    const filteredData = foodSales.filter(sale => {
      const orderDate = new Date(sale.orderDate);
      const start = new Date(startDate);
      const end = new Date(endDate);
      return orderDate >= start && orderDate <= end;
    });
    setFoodSales(filteredData);
  };

  // Handle sorting
  const sortBy = (column) => {
    const sortedData = [...foodSales].sort((a, b) => {
      if (a[column] < b[column]) return -1;
      if (a[column] > b[column]) return 1;
      return 0;
    });
    setFoodSales(sortedData);
  };

  return (
    <div className="App">
      <h1 className="title">Food Sales Data</h1>

      <div className="filters">
        <input
          type="text"
          className="search-input"
          placeholder="Search by product"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
        <input
          type="date"
          className="date-input"
          value={startDate}
          onChange={(e) => setStartDate(e.target.value)}
        />
        <input
          type="date"
          className="date-input"
          value={endDate}
          onChange={(e) => setEndDate(e.target.value)}
        />
        <button className="filter-button" onClick={filterByDate}>Filter by Date</button>
      </div>

      <table className="data-table">
        <thead>
          <tr>
            <th onClick={() => sortBy('orderDate')}>Order Date</th>
            <th onClick={() => sortBy('region')}>Region</th>
            <th onClick={() => sortBy('city')}>City</th>
            <th onClick={() => sortBy('category')}>Category</th>
            <th onClick={() => sortBy('product')}>Product</th>
            <th onClick={() => sortBy('quantity')}>Quantity</th>
            <th onClick={() => sortBy('unitPrice')}>Unit Price</th>
            <th onClick={() => sortBy('totalPrice')}>Total Price</th>
          </tr>
        </thead>
        <tbody>
          {foodSales.filter(sale =>
            sale.product.toLowerCase().includes(search.toLowerCase())
          ).map((sale, index) => (
            <tr key={index}>
              <td>{new Date(sale.orderDate).toLocaleDateString()}</td>
              <td>{sale.region}</td>
              <td>{sale.city}</td>
              <td>{sale.category}</td>
              <td>{sale.product}</td>
              <td>{sale.quantity}</td>
              <td>{sale.unitPrice}</td>
              <td>{sale.totalPrice}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default App;
