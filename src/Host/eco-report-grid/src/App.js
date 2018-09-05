import React, { Component, Fragment } from "react";
import { Table } from "reactstrap";
import axios from "axios";

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
  @typedef {Object} State
  @property {Report[]} reports
  @property {number} index
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
 */
function createTable(report, index) {
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
                Activity Perform (Period)
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
    /** @type {State} */
    this.state = {
      reports: []
    };
  }

  componentDidMount() {
    axios.get("http://localhost:5000/Company/data").then(json => {
      this.setState({
        reports: [...json.data]
      });
    });
  }

  render() {
    const tables = this.state.reports.map((report, index) =>
      createTable(report, index)
    );
    return <div>{tables}</div>;
  }
}

export default App;
