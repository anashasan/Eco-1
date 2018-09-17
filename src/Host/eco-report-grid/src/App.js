import React, { Component, Fragment } from "react";
import {
  Table,
  Container,
  Row,
  Form,
  FormGroup,
  Label,
  Input,
  Card,
  CardHeader,
  CardBody,
  Button
} from "reactstrap";
import queryString from "query-string";
import { ApiClient } from "./ApiClient";

/**
  @typedef {Object} ActivityDetail
  @property {boolean} isPerform  
  @property {string} perform
  @property {string} activityName
 */

/**
  @typedef {Object} DailyActivityPerform
  @property {number} stationNo
  @property {string} locationName
  @property {ActivityDetail[]} activityPerform
 */

/**
  @typedef {Object} Report
  @property {string} stationName
  @property {DailyActivityPerform[]} dailyActivityPerformReport
  @property {string[]} activities
 */

/**
  @typedef {Object} Location
  @property {number} locationId
  @property {string} locationName
 */

/**
  @typedef {Object} State
  @property {Report[]} reports
  @property { Date } createdOn
  @property { Number } locationId
  @property { Number } branchId
  @property {Location[]} locations
 */

/**
 * @param {number} length
 */
function createEmptyTd(length) {
  const elements = [];
  let counter = length;
  for (let index = 0; index < length; index++) {
    elements.push(
      <td rowSpan="1" colSpan="1" key={counter + "-em"}>
        &nbsp;
      </td>
    );
    counter++;
  }

  return elements;
}

/**
 * @param {ActivityDetail[]} activityDetail
 * @param {string[]} activities
 */
function createActivities(activityDetail, activities) {
  if (activityDetail.length === 0) return null;

  const activitiesElements = [];
  for (let index = 0; index < activities.length; index++) {
    const activity = activities[index];
    const activityInfo = activityDetail.find(i => i.activityName === activity);
    if (activityInfo) {
      activitiesElements.push(
        <td rowSpan="1" colSpan="1" key={index}>
          {activityInfo.perform}
        </td>
      );
    } else {
      activitiesElements.push(
        <td rowSpan="1" colSpan="1" key={index}>
          &nbsp;
        </td>
      );
    }
  }

  return activitiesElements;
}

/**
 * @param {Report} report
 * @param {Date} createdOn
 * @param {Number} index
 */
function createTable(report, createdOn, index) {
  const trData = report.dailyActivityPerformReport.map(
    (dailyActivityPerform, index) => (
      <tr key={index}>
        <td rowSpan="1" colSpan="1">
          {dailyActivityPerform.stationNo}
        </td>
        <td rowSpan="1" colSpan="1">
          {dailyActivityPerform.locationName}
        </td>
        {dailyActivityPerform.activityPerform &&
          createActivities(
            dailyActivityPerform.activityPerform,
            report.activities
          )}
        {!dailyActivityPerform.activityPerform &&
          createEmptyTd(report.activities.length)}
      </tr>
    )
  );

  return (
    <Fragment key={index}>
      <h4>{report.stationName}</h4>
      <Table className="text-center" striped bordered responsive>
        <tbody>
          <tr className="table-secondary">
            <td rowSpan="1" colSpan="2">
              Station Details
            </td>
            {report.activities && (
              <td rowSpan="1" colSpan={report.activities.length}>
                Activity Perform ({createdOn})
              </td>
            )}
          </tr>
          <tr>
            <td rowSpan="1" colSpan="1">
              Station No
            </td>
            <td rowSpan="1" colSpan="1">
              Location Name
            </td>
            {report.activities &&
              report.activities.map((activity, index) => (
                <td rowSpan="1" colSpan="1" key={index}>
                  {activity}
                </td>
              ))}
          </tr>
          {trData}
        </tbody>
      </Table>
    </Fragment>
  );
}

class App extends Component {
  constructor() {
    super();
    const parsed = queryString.parse(window.location.search);
    /** @type {State} */
    this.state = {
      reports: [],
      createdOn: "",
      locationId: 0,
      branchId: parsed.branchId,
      locations: []
    };

    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  componentDidMount() {
    ApiClient.get(`/Company/data?branchId=${this.state.branchId}`).then(
      json => {
        this.setState({
          reports: [...json.data]
        });
      }
    );

    ApiClient.get(`/Company/locations/branchId/${this.state.branchId}`).then(
      json => {
        this.setState({
          locations: [...json.data]
        });
      }
    );
  }

  handleChange(event) {
    var name = event.target.name;
    this.setState({ [name]: event.target.value });
  }

  handleSubmit(event) {
    ApiClient.get(
      `/Company/data?branchId=${this.state.branchId}&locationId=${
        this.state.locationId === 0 ? null : this.state.locationId
      }&createdOn=${this.state.createdOn}`
    ).then(json => {
      this.setState({
        reports: [...json.data]
      });
    });
    event.preventDefault();
  }

  render() {
    const tables = this.state.reports.map((report, index) =>
      createTable(report, this.state.createdOn, index)
    );
    return (
      <Container>
        <br />
        <Row>
          <Card>
            <CardHeader>Search Box</CardHeader>
            <CardBody>
              <Form inline onSubmit={this.handleSubmit}>
                <FormGroup className="mb-2 mr-sm-2 mb-sm-0">
                  <Label for="txtCreatedOn" className="mr-sm-2">
                    Created On
                  </Label>
                  <Input
                    type="date"
                    id="txtCreatedOn"
                    name="createdOn"
                    value={this.state.createdOn}
                    onChange={this.handleChange}
                  />
                </FormGroup>
                <FormGroup className="mb-2 mr-sm-2 mb-sm-0">
                  <Label for="locationDropDown" className="mr-sm-2">
                    Location
                  </Label>
                  <Input
                    type="select"
                    id="locationDropDown"
                    name="locationId"
                    value={this.state.locationId}
                    onChange={this.handleChange}
                  >
                    <option value={null}>Select All</option>
                    {this.state.locations.map((locationData, index) => (
                      <option key={index} value={locationData.locationId}>
                        {locationData.locationName}
                      </option>
                    ))}
                  </Input>
                </FormGroup>
                <Button>Search</Button>
              </Form>
            </CardBody>
          </Card>
        </Row>
        <br />
        <Row>
          <div>{tables}</div>
        </Row>
      </Container>
    );
  }
}

export default App;
