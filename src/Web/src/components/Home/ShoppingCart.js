import React, { Component } from "react";
import { Link } from "react-router";
import {
  Button,
  Card,
  CardText,
  CardBlock,
  CardTitle,
  CardSubtitle,
  Container,
  Row,
  Col,
  Table,
  Input
} from "reactstrap";

export default class ShoppingCart extends Component {
  render() {
    const {
      products,
      handleEmptyCartClick,
      handleRemoveProductFromCart,
      isConfirmPage
    } = this.props;
    let money = 0;
    return (
      <div>
        <Row>
          <Col xs="9">
            <div>
              <Table striped responsive hover>
                <thead>
                  <tr>
                    <td>#</td>
                    <td>Name</td>
                    <td>Model</td>
                    <td>Price</td>
                    <td>Quantity</td>
                    <td>&nbsp;</td>
                  </tr>
                </thead>
                <tbody>
                  {products.map((p, index) => {
                    money += p.product.price * p.quantity;
                    return (
                      <tr key={p.product.id}>
                        <td>{index + 1}</td>
                        <td>{p.product.name}</td>
                        <td>{p.product.model}</td>
                        <td>${p.product.price}</td>
                        <td>{p.quantity}</td>
                        <td>
                          {!isConfirmPage &&
                            <Button
                              onClick={() =>
                                handleRemoveProductFromCart(p.product)}
                              size="sm"
                              color="warning"
                            >
                              Remove
                            </Button>}
                        </td>
                      </tr>
                    );
                  })}
                  <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td><b>Total</b>:</td>
                    <td><b>${money}</b></td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                  </tr>
                </tbody>
              </Table>
            </div>
          </Col>
          <Col xs="3">
            {!isConfirmPage &&
              products.length > 0 &&
              <Button color="danger" onClick={() => handleEmptyCartClick()}>
                Empty cart
              </Button>}
            {!isConfirmPage &&
              products.length > 0 &&
              <Button color="info">
                <Link to="/checkout">Checkout</Link>
              </Button>}
          </Col>
        </Row>
      </div>
    );
  }
}
