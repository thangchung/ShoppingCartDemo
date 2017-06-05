import React, { Component } from "react";
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

export default class ThreeItems extends Component {
  constructor() {
    super();
    this.state = {
      valueChange: 1
    };
  }

  shouldComponentUpdate(nextProps, nextState) {
    return false;
  }

  onQuantityChange(value) {
    this.state.valueChange = value;
  }

  render() {
    const { products, handleAddProductClick } = this.props;

    return (
      <div>
        <Row>
          {products.map(product => (
            <Col key={product.id} xs="4">
              <Card>
                <CardBlock>
                  <CardTitle>{product.name} </CardTitle>
                  <CardSubtitle>{product.model}</CardSubtitle>
                  <CardText>Price: ${product.price}</CardText>
                  <Row>
                    <Col xs="6">
                      <Input
                        type="textbox"
                        defaultValue={this.state.valueChange}
                        onChange={this.onQuantityChange.bind(this)}
                      />
                    </Col>
                    <Col xs="6">
                      <Button
                        color="success"
                        onClick={() =>
                          handleAddProductClick(
                            product,
                            this.state.valueChange
                          )}
                      >
                        Add to cart
                      </Button>
                    </Col>
                  </Row>
                </CardBlock>
              </Card>
            </Col>
          ))}
        </Row>
      </div>
    );
  }
}
